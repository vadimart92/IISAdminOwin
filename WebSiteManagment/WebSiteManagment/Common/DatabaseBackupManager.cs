using System;
using Microsoft.SqlServer.Management.Smo;

namespace WebSiteManagment.Core.Common
{
	public class DatabaseBackupManager
	{
		public static void RestoreDbBackup(string backupFileName, string destinationDbName, string serverName, string instanceName) {
			//todo: restore db
			throw new NotImplementedException();
			var x = new Microsoft.SqlServer.Management.Smo.Restore();
		}
	}
}