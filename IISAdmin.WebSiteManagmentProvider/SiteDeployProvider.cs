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

	    private Task RestoreBD(ISqlServerInstance sqlServer, string backupPath, string name) {
			return DatabaseBackupManager.RestoreDbBackupAsync(name);
		}

		private Task ExtractDBBackup(string zipFilePath, string backupPath) {

	    }

	    public void DeployWebApp(ISiteCreateData siteCreateData, IProgress<ISiteDeployProgress> progress, ISiteDeployProgress initialProgress) {
		    var extractionTask = BuildExtractor.ExtractBuildAsync(siteCreateData.DestinationPath, siteCreateData.Name);
			var extractBakTask = ExtractDBBackup();
			var restoreTask = RestoreBD();
		    extractBakTask.ContinueWith((task, o) => {
			    restoreTask.RunSynchronously();
		    }, null, TaskContinuationOptions.OnlyOnRanToCompletion);
		    Action<Task> reportProgress = (task) => {
			    initialProgress.SetNextOperation();
			    progress.Report(initialProgress);
		    };
			Task.WaitAll(extractionTask.ContinueWith(reportProgress), restoreTask.ContinueWith(reportProgress));
	    }
		
	    public ISiteDeployProgress GetInitDeployProgress(IEnumerable<DeployOperationIfo> extraOperations) {
			var operations = new List<DeployOperationIfo> {
				new DeployOperationIfo{Info = "Copy files / Restore DB"},
				new DeployOperationIfo{Info = "Modify configs"}
			};
			return new SiteDeployProgress(operations.Concat(extraOperations));
	    }
    }

	
}
