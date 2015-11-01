using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        private readonly IJobInfoRepository _jobInfoRepository;
        private readonly IHubContextProvider _hubContextProvider;

        public SiteDeployProvider(ISiteDeployProviderWebConfig config, IWebSiteRepository siteRepository, IJobInfoRepository jobInfoRepository, IHubContextProvider hubContextProvider) {
		    _config = config;
		    _siteRepository = siteRepository;
            _jobInfoRepository = jobInfoRepository;
            _hubContextProvider = hubContextProvider;
        }

	    private Task ExtractBuildTask(SiteCreateData siteCreateData) {
			return new Task(() => {
				ZipExtractor.ExtractBuild(siteCreateData.ReleaseInfo.ZipFilePath, siteCreateData.DestinationWebAppRoot, new List<string> { ".bak" });
			});
	    }
		
		private Task ExtractDBBackup(string zipFilePath, string dbFileDirectory, string dbFileName, string backupPath) {
			return new Task(() => {
				ZipExtractor.ExtractFileFromZip(zipFilePath, dbFileDirectory, dbFileName, backupPath);
			});
	    }
        
        public OperationInfoBase GetOperationsInfo() {
            var info = new DeploySiteInfo(_jobInfoRepository, _hubContextProvider);
            return info.Instance;
        }

        public void DeployWebApp(SiteCreateData siteCreateData, Guid jobInfoId) {
            Contract.Requires(jobInfoId != Guid.Empty, "jobInfoId != Guid.Empty");
            var deployInfo = new DeploySiteInfo(_jobInfoRepository, _hubContextProvider, jobInfoId);
            InitDeployInfo(siteCreateData);
			ExtractBinaries(siteCreateData);
		    deployInfo.RestoreDbCopyFiles = OperationStageState.Completed;
            if (_config.NewSiteForBuild) {
				siteCreateData.CreateNewSite = true;
			}
			var siteId = _siteRepository.CreateSite(siteCreateData, 1);
            deployInfo.CreateWebApp = OperationStageState.Completed;
			var site = _siteRepository.GetSite(siteId);
			InitRedisInfo(siteCreateData, site);
			_siteRepository.ModifyConnectionStrings(siteId, new Dictionary<string, string> {
				{"redis", siteCreateData.RedisInfo.ConnectionString},
				{"db", GetDbConnectionString(siteCreateData, site.Applications.First().DbConnectionString)}
			});
            deployInfo.ModifyConfigs = OperationStageState.Completed;
        }

		private void ExtractBinaries(SiteCreateData siteCreateData) {
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
        
        public void InitDeployInfo(SiteCreateData siteCreateData) {
			var info = SiteDeployNamesHelper.GetDeployNamesInfo(siteCreateData, _config.WebAppRoot);
			siteCreateData.WebAppName = info.WebAppName;
			siteCreateData.DestinationWebAppRoot = siteCreateData.SeparateFolder? Path.Combine(_config.WebAppRoot, info.ShortVersion, info.ProductName) : _config.WebAppRoot + "\\" + info.WebAppName;
		}
        
        public void InitRedisInfo(SiteCreateData siteCreateData, ISite siteData) {
		    siteCreateData.RedisInfo = string.IsNullOrWhiteSpace(_config.RedisTypicalConnectionString)
			    ? new Redis(siteData.Applications.First().Redis.ConnectionString)
			    : new Redis(_config.RedisTypicalConnectionString);
			siteCreateData.RedisInfo.Db = GetFreeRedisDb(siteCreateData.RedisInfo.Host);
	    }

	    private string GetDbConnectionString(SiteCreateData siteCreateData, string oldValue) {
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
				var siteExists = sites.Where(s => s.Applications.First().Redis.Host == host && (port == null || s.Applications.First().Redis.Port == port))
					.Any(s => s.Applications.First().Redis.Db == number);
				if (siteExists) {
					continue;
				}
				numberFound = true;
			} while (!numberFound);
			return number;
		}

    }
}
