using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IISAdmin.Interfaces;
using WebSiteManagment.Core.Common;

namespace IISAdmin.WebSiteManagmentProvider
{
    public class SiteDeployProvider : ISiteDeployProvider {

	    private readonly ISiteDeployProviderWebConfig _config;
		private readonly IWebSiteRepository _siteRepository;

	    public SiteDeployProvider(ISiteDeployProviderWebConfig config, IWebSiteRepository siteRepository) {
		    _config = config;
		    _siteRepository = siteRepository;
	    }

	    private Task ExtractBuildTask(ISiteCreateData siteCreateData) {
			return new Task(() => {
				ZipExtractor.ExtractBuild(siteCreateData.ReleaseInfo.ZipFilePath, siteCreateData.DestinationWebAppRoot, new List<string> { ".bak" });
			});
	    }
		
		private Task ExtractDBBackup(string zipFilePath, string dbFileDirectory, string dbFileName, string backupPath) {
			return new Task(() => {
				ZipExtractor.ExtractFileFromZip(zipFilePath, dbFileDirectory, dbFileName, backupPath);
			});
	    }

		public void DeployWebApp(ISiteCreateData siteCreateData, ISiteDeployProgress progressInfo) {
			progressInfo.SetNextOperation();
			InitDeployInfo(siteCreateData);
			ExtractBinaries(siteCreateData);
			progressInfo.SetNextOperation();
			if (_config.NewSiteForBuild) {
				siteCreateData.CreateNewSite = true;
			}
			var siteId = _siteRepository.CreateSite(siteCreateData, 1);
			var site = _siteRepository.GetSite(siteId);
			InitRedisInfo(siteCreateData, site);
			_siteRepository.ModifyConnectionStrings(siteId, new Dictionary<string, string> {
				{"redis", siteCreateData.RedisInfo.ConnectionString},
				{"db", GetDbConnectionString(siteCreateData, site.DbConnectionString)}
			});
	    }

		private void ExtractBinaries(ISiteCreateData siteCreateData) {
			var extractionTask = ExtractBuildTask(siteCreateData);
			var backupPath = _config.DbBackupTempPath + "\\" + siteCreateData.WebAppName + ".bak";
			var extractDbBackup = ExtractDBBackup(siteCreateData.ReleaseInfo.ZipFilePath, "db", ".bak", backupPath);
			var restoreTask = extractDbBackup.ContinueWith((task) => {
				DatabaseBackupManager.RestoreDbBackup(backupPath, siteCreateData.WebAppName, siteCreateData.Db.ServerName, siteCreateData.Db.InstanceName);
				File.Delete(backupPath);
			}, TaskContinuationOptions.OnlyOnRanToCompletion);
			extractDbBackup.Start();
			extractionTask.Start();
			Task.WaitAll(restoreTask, extractionTask);
		}

	    public ISiteDeployProgress GetInitDeployProgress(IEnumerable<DeployOperationIfo> extraOperations) {
			var operations = new List<DeployOperationIfo> {
				new DeployOperationIfo{Info = "Restore DB / Copy files"}
			};
			return new SiteDeployProgress(operations.Concat(extraOperations));
	    }

		public void InitDeployInfo(ISiteCreateData siteCreateData) {
			var info = SiteDeployNamesHelper.GetDeployNamesInfo(siteCreateData, _config.WebAppRoot);
			siteCreateData.WebAppName = info.WebAppName;
			siteCreateData.DestinationWebAppRoot = siteCreateData.SeparateFolder? Path.Combine(_config.WebAppRoot, info.ShortVersion, info.ProductName) : _config.WebAppRoot + "\\" + info.WebAppName;
		}

	    public void InitRedisInfo(ISiteCreateData siteCreateData, ISite siteData) {
		    siteCreateData.RedisInfo = string.IsNullOrWhiteSpace(_config.RedisTypicalConnectionString)
			    ? new Redis(siteData.Redis.ConnectionString)
			    : new Redis(_config.RedisTypicalConnectionString);
			siteCreateData.RedisInfo.Db = GetFreeRedisDb(siteCreateData.RedisInfo.Host);
	    }

	    private string GetDbConnectionString(ISiteCreateData siteCreateData, string oldValue) {
		    SqlConnectionStringBuilder builder;
		    try {
			    builder = new SqlConnectionStringBuilder(oldValue);
		    } catch (Exception) {
			    builder = new SqlConnectionStringBuilder(_config.DbTypicalConnectionString);
		    }
		    builder.DataSource = string.IsNullOrWhiteSpace(siteCreateData.Db.InstanceName)
			    ? siteCreateData.Db.ServerName
			    : String.Format(@"{0}\{1}", siteCreateData.Db.ServerName, siteCreateData.Db.InstanceName);
		    builder.InitialCatalog = siteCreateData.WebAppName;
			return builder.ConnectionString;
		}

		private int GetFreeRedisDb(string host, int? port = null) {
			_siteRepository.ClearSiteCache();
			var sites = _siteRepository.GetAllSites();
			var numberFound = false;
			var number = -1;
			do {
				number++;
				var siteExists = sites.Where(s => s.Redis.Host == host && (port == null || s.Redis.Port == port))
					.Any(s => s.Redis.Db == number);
				if (siteExists) {
					continue;
				}
				numberFound = true;
			} while (!numberFound);
			return number;
		}

    }
}
