using System;
using System.Data.SqlClient;
using IISAdmin.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IISAdmin.Owin.DAL.Test
{
	[TestClass]
    public class WorkDbReleaseRepositoryTest
    {
		[TestMethod]
		public void WorkDbReleaseRepository_Get() {
			var mock = new Mock<ISqlConnectionProvider>(MockBehavior.Default);
			var sqlConn = new SqlConnection(@"Server=ArtemchukPC;Integrated Security=SSPI;Database=work30");
			mock.Setup((prov) => prov.ExecuteAction(It.IsAny<Func<SqlConnection, IRelease>>(), null)).Callback<Func<SqlConnection, IRelease>, SqlCredential>((action, credential) => action(sqlConn));
			IReleaseRepository releaseRepository = new WorkDbReleaseRepository.WorkDbReleaseRepository(mock.Object);
			var release = releaseRepository.Get(Guid.Empty);
			Assert.IsNull(release);
		}
	}
}
