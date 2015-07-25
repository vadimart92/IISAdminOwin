using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSiteManagment.Core.Common;

namespace WebSiteManagment.Core.Test {
	[TestClass]
	public class DatabaseBackupManagerTest {
		[TestMethod]
		public void RestoreDbBackup_if_dbExists() {
			var name = Path.GetRandomFileName();
			using (var connection = new SqlConnection(@"Server=ArtemchukPC;Integrated Security=SSPI;")) {
				try {
					DatabaseBackupManager.RestoreDbBackup("test.bak", name, "ArtemchukPC", "");
					connection.Open();
					var dbName = new {
						dbName = name
					};
					var databases =
						connection.Query<int?>(@"SELECT 1 FROM master.sys.databases d WHERE d.name = @dbName", dbName);
					Assert.IsTrue(databases.FirstOrDefault() == 1);
				} finally {
					connection.Query(string.Format(@"DROP DATABASE [{0}]", name));
				}
			}
		}
        [TestMethod]
		public void DropDatabase_WithHistory() {
			var name = Path.GetRandomFileName();
			using (var connection = new SqlConnection(@"Server=ArtemchukPC;Integrated Security=SSPI;")) {
				try {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $@"CREATE DATABASE [{name}];";
                    cmd.ExecuteNonQuery();
                    DatabaseBackupManager.DropDatabase(name, "ArtemchukPC", "");
					var dbName = new {
						dbName = name
					};
					var databases =
						connection.Query<int?>(@"SELECT 1 FROM master.sys.databases d WHERE d.name = @dbName", dbName);
					Assert.IsNull(databases.FirstOrDefault());
				} finally {
					connection.Query($@"BEGIN TRY DROP DATABASE [{name}] END TRY BEGIN CATCH END CATCH");
				}
			}
		}
        [TestMethod]
		public void DropDatabase_WithoutHistory() {
			var name = Path.GetRandomFileName();
			using (var connection = new SqlConnection(@"Server=ArtemchukPC;Integrated Security=SSPI;")) {
				try {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $@"CREATE DATABASE [{name}];";
                    cmd.ExecuteNonQuery();
                    DatabaseBackupManager.DropDatabase(name, "ArtemchukPC", "", false);
					var dbName = new {
						dbName = name
					};
					var databases =
						connection.Query<int?>(@"SELECT 1 FROM master.sys.databases d WHERE d.name = @dbName", dbName);
                    Assert.IsFalse(databases.Any());
                } finally {
                    connection.Query($@"BEGIN TRY DROP DATABASE [{name}] END TRY BEGIN CATCH END CATCH");
                }
			}
		}
	}
}
