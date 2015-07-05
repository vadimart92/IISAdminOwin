using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISAdmin.Interfaces;
using WebSiteManagment.Core.Common;

namespace IISAdmin.WebSiteManagmentProvider
{
    public class SiteDeployProvider : ISiteDeployProvider
    {

		private Task ExtractBuildTask(string zipFile, string destinationPath) {
			return new Task(() => {
				ZipExtractor.ExtractBuild(zipFile, destinationPath, new List<string>{".bak"});
			});
		}

	    private Task RestoreBD(ISqlServerInstance sqlServer, string backupPath, string name) {
			return new Task(() => {
				DatabaseBackupManager.RestoreDbBackup(backupPath, name, sqlServer.ServerName, sqlServer.InstanceName);
			});
		}

		private Task ExtractDBBackup(string zipFilePath, string dbFileDirectory, string dbFileName, string backupPath) {
			return new Task(() => {
				ZipExtractor.ExtractFileFromZip(zipFilePath, dbFileDirectory, dbFileName, backupPath);
			});
	    }

	    public void DeployWebApp(ISiteCreateData siteCreateData, IProgress<ISiteDeployProgress> progress, ISiteDeployProgress initialProgress) {
			var extractionTask = ExtractBuildTask(siteCreateData.ReleaseInfo.ZipFilePath, siteCreateData.DestinationPath);
			var backupPath = siteCreateData.DbBackupTempPath + siteCreateData.Name + ".bak";
			var restoreTask = ExtractDBBackup(siteCreateData.ReleaseInfo.ZipFilePath, "db", @".bak", backupPath).ContinueWith((task, o) => {
			    RestoreBD(siteCreateData.Db, backupPath, siteCreateData.Name).RunSynchronously();
		    }, null, TaskContinuationOptions.OnlyOnRanToCompletion);
			restoreTask.Start();
			extractionTask.Start();
			Task.WaitAll(restoreTask, extractionTask);
			initialProgress.SetNextOperation();
			progress.Report(initialProgress);
	    }
		
	    public ISiteDeployProgress GetInitDeployProgress(IEnumerable<DeployOperationIfo> extraOperations) {
			var operations = new List<DeployOperationIfo> {
				new DeployOperationIfo{Info = "Restore DB / Copy files"},
				new DeployOperationIfo{Info = "Modify configs"}
			};
			return new SiteDeployProgress(operations.Concat(extraOperations));
	    }
    }

	
}
