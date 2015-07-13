using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using IISAdmin.Interfaces;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;

namespace IISAdmin.Owin.DAL
{
	public class SqlServerInstance : ISqlServerInstance {
		public string ServerName { get; set; }

		public string InstanceName { get; set; }

		public string Version { get; set; }

		public string Name {
			get {
				return (new List<string> { ServerName, InstanceName }).Where(s => !string.IsNullOrWhiteSpace(s)).Aggregate((a, b) => a + "/" + b);
			}
		}
	}

	public class LocalSqlServerInstanceRepository : ISqlServerInstanceRepository
	{
		private static readonly Lazy<DataTable> _instanceDataTable = new Lazy<DataTable>(() => SqlDataSourceEnumerator.Instance.GetDataSources());

		private static DataTable InstanceDataTable {
			get { return _instanceDataTable.Value; }
		}

		#region Члены ISqlServerInstanceRepository

		public IList<ISqlServerInstance> GetAllInstances(IList<string> servernameFilter = null) {
			return GetServerInstances(servernameFilter).ConvertAll(i => (ISqlServerInstance)i);
		}

		private List<SqlServerInstance> GetFromDataTable(DataTable table, string snColumnName, string inColumnName, string vColumnName) {
			var snColumn = table.Columns[snColumnName];
			var inColumn = table.Columns[inColumnName];
			var vColumn = table.Columns[vColumnName];
			var res = table.Select("len("+snColumn+")>0")
						   .Select(row => new SqlServerInstance {
							   ServerName = (string)row[snColumn],
							   InstanceName = (string)row[inColumn],
							   Version = (string)row[vColumn]
						   }).ToList();
			return res;
		}

		#endregion Члены ISqlServerInstanceRepository

		private List<ISqlServerInstance> GetServerInstances(IList<string> serverNames) {
			var table = InstanceDataTable.Copy();
			var list = GetFromDataTable(table, "ServerName", "InstanceName", "Version");
			var smoList = GetFromDataTable(SmoApplication.EnumAvailableSqlServers(), "Server", "Instance", "Version");
			var res = list.Concat(smoList).Where(r => serverNames == null || serverNames.Contains(r.ServerName)).Concat(GetServerInstancesMock()).ToList();
			return res.Distinct(SqlServerInstanceComparer.Instance).ToList();
		}

		private List<SqlServerInstance> GetServerInstancesMock() {
			return JsonConvert.DeserializeObject<List<SqlServerInstance>>("[{\"ServerName\":\"ArtemchukPC\",\"InstanceName\":\"\",\"Name\":\"ArtemchukPC\",\"Version\":\"12.00.2000\"},{\"ServerName\":\"ArtemchukPC\",\"InstanceName\":\"\",\"Name\":\"ArtemchukPC\",\"Version\":\"12.00.2000\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2005\",\"Name\":\"TSCDISTR-MS/MSSQL2005\",\"Version\":\"9.00.5000.00\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2008\",\"Name\":\"TSCDISTR-MS/MSSQL2008\",\"Version\":\"10.0.5500.0\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2008R2\",\"Name\":\"TSCDISTR-MS/MSSQL2008R2\",\"Version\":\"10.50.2500.0\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2012\",\"Name\":\"TSCDISTR-MS/MSSQL2012\",\"Version\":\"11.0.5058.0\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2014\",\"Name\":\"TSCDISTR-MS/MSSQL2014\",\"Version\":\"12.0.2000.8\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQLEXPRESS2014\",\"Name\":\"TSCDISTR-MS/MSSQLEXPRESS2014\",\"Version\":\"12.0.2000.8\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2005\",\"Name\":\"TSCPSF-MS/MSSQL2005\",\"Version\":\"9.00.5000.00\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2008\",\"Name\":\"TSCPSF-MS/MSSQL2008\",\"Version\":\"10.0.5500.0\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2008R2\",\"Name\":\"TSCPSF-MS/MSSQL2008R2\",\"Version\":\"10.50.2500.0\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2012\",\"Name\":\"TSCPSF-MS/MSSQL2012\",\"Version\":\"11.0.5058.0\"}]");
		}
	}
}