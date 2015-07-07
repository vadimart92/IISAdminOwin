using System;
using DeclarativeSql;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using DeclarativeSql.Dapper;

namespace IISAdmin.Owin.DAL.Test {
	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void TestMethod1() {
			ISqlConnectionProvider connectionProvider = new WorkSqlConnectionProvider(@"Server=ArtemchukPC;Integrated Security=SSPI;");
			connectionProvider.ExecuteAction((connection) => {
				var text = connection.Select<WorkDbRelease>(r=>r.Id == Guid.NewGuid());
			});
		}
	}
}
