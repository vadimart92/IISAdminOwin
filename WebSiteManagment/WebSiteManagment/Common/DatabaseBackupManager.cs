using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace WebSiteManagment.Core.Common
{
	public class DatabaseBackupManager
	{
		public static void RestoreDbBackup(string backupFileName, string destinationDbName, string serverName, string instanceName,
				IProgress<RestoreProgress> progress = null) {
			Contract.Ensures(File.Exists(backupFileName), "File.Exists(backupFileName)");
			var restoreDb = new Restore {
				Database = destinationDbName,
				Action = RestoreActionType.Database,
				ReplaceDatabase = true,
				NoRecovery = false
			};
			if (progress != null) {
				restoreDb.PercentComplete += (sender, args) => progress.Report(new RestoreProgress {
					Percent = args.Percent, Message = args.Message
				});
				restoreDb.Complete += (sender, args) => progress.Report(new RestoreProgress {
					Percent = 100, Message = args.ToString(), SqlError = args.Error
				});
			}
			restoreDb.Devices.AddDevice(backupFileName, DeviceType.File);
			using (var connection = GetConnection(serverName, instanceName)) {
				connection.Open();
				var server = new Server(new ServerConnection(connection));
				var relocateFiles = GetRelocateFiles(connection, backupFileName, destinationDbName, server.DefaultFile, server.DefaultLog);
				restoreDb.RelocateFiles.AddRange(relocateFiles);
				restoreDb.SqlRestore(server);
			}
		}

		private static SqlConnection GetConnection(string serverName, string instanceName) {
			return
				new SqlConnection(String.Format(@"Server={0}\{1};Integrated Security=SSPI;Database=master", serverName, instanceName));
		}

		public static bool DropDatabase(string name, string serverName, string instanceName, bool dropBackupHistory = true) {
			using (var connection = GetConnection(serverName, instanceName)) {
				connection.Open();
				var server = new Server(new ServerConnection(connection));
				if (server.Databases.Contains(name)) {
					var db = server.Databases[name];
					try {
						if (dropBackupHistory) {
							db.DropBackupHistory();
						}
						db.Drop();
					}
					catch (Exception) {
						var cmd = connection.CreateCommand();
						cmd.Connection = connection;
						cmd.CommandText = String.Format(@"USE master; ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{0}];", name);
						cmd.ExecuteNonQuery();
					}
					return true;
				}
			}
			return false;
		}

		private static List<RelocateFile> GetRelocateFiles(SqlConnection connection, string backupFileName, string name, string defaultFile, string defaultLog) {
			var res = new List<RelocateFile>();
			var command = new SqlCommand(@"RESTORE FILELISTONLY FROM DISK = @file"){Connection = connection};
			command.Parameters.AddWithValue("@file", backupFileName);
			using (var reader = command.ExecuteReader(CommandBehavior.Default)) {
				while (reader.Read()) {
					var type = reader.GetString(reader.GetOrdinal("Type"));
					string newName;
					switch (type) {
						case "D":
							newName = defaultFile + name + "_" + type + (type == "D" ? ".mdf" : ".ldf");
							break;
						case "L":
							newName = defaultLog + name + "_" + type + (type == "D" ? ".mdf" : ".ldf");
							break;
						default:
							newName = defaultFile + name + "_" + type + Path.GetRandomFileName()+ (type == "D" ? ".mdf" : ".ldf");
							break;
					}
					
					res.Add(new RelocateFile(reader.GetString(reader.GetOrdinal("LogicalName")), newName));
				}
			}
			return res;
		}
	}
	public class RestoreProgress {
		public int Percent { get; set; }
		public string Message { get; set; }
		public SqlError SqlError { get; set; }
	}
}