﻿using System;
using System.Data;
using System.Data.SqlClient;
using DeclarativeSql;
using IISAdmin.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using DeclarativeSql.Dapper;
using IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository;

namespace IISAdmin.Owin.DAL.Test {
	[TestClass]
	public class WorkSqlConnectionProviderTest {
		[TestMethod]
		public void ExecuteAction() {
			ISqlConnectionProvider connectionProvider = new WorkSqlConnectionProvider(@"Server=ArtemchukPC;Integrated Security=SSPI;");
			connectionProvider.ExecuteAction((connection) => {
				Assert.IsNotNull(connection);
                Assert.IsInstanceOfType(connection, typeof(SqlConnection));
                Assert.IsTrue(connection.State == ConnectionState.Open);
			});
		}
	}
}
