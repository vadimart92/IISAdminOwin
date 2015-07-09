using System;
using System.Data.SqlClient;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebSiteManagment.Core.Common;

namespace IISAdmin.Owin.DAL.Test
{
	[TestClass]
    public class WorkDbReleaseRepositoryTest
    {
		[TestInitialize]
		public void Init() {
			DatabaseBackupManager.RestoreDbBackup("terrasoft_work30.bak", "terrasoft_work30", "ArtemchukPC", "");
		}

		[TestCleanup]
		public void TearDown() {
			DatabaseBackupManager.DropDatabase("terrasoft_work30", "ArtemchukPC", "");
		}

		[TestMethod]
		public void WorkDbReleaseRepository_Get() {
			var mock = new Mock<ISqlConnectionProvider>();
			var sqlConn = new SqlConnection(@"Server=ArtemchukPC;Integrated Security=SSPI;Database=terrasoft_work30");
			mock.Setup((prov) => prov.ExecuteAction(It.IsAny<Func<SqlConnection, WorkDbRelease>>(), It.IsAny<SqlCredential>()))
				.Returns((Func<SqlConnection, WorkDbRelease> action, SqlCredential credential) => action(sqlConn));
			IReleaseRepository releaseRepository = new WorkDbReleaseRepository.WorkDbReleaseRepository(mock.Object);
			var release = releaseRepository.Get(new Guid("DD5AD0ED-4ABF-44AF-A851-5127A1389FD8"));
			sqlConn.Dispose();
			Assert.IsNotNull(release);
		}
	}
}
