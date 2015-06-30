using System;
using System.Threading.Tasks;

namespace WebSiteManagment.Core.Common
{
	public class DatabaseBackupManager
	{
		public static Task RestoreDbBackupAsync(string backupFileName, string destinationDbName, string serverName, string instanceName) {
			//todo: restore db
			return Task.Run(()=>{
				throw new NotImplementedException();
			});
		}
	}
}