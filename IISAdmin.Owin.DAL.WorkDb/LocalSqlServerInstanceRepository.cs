using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using IISAdmin.Interfaces;
using Newtonsoft.Json;

namespace IISAdmin.Owin.DAL
{
	public class SqlServerInstance : ISqlServerInstance
	{
		public string ServerName { get; set; }

		public string InstanceName { get; set; }

		public string Version { get; set; }

		public string Name {
			get {
				return string.Format("{0}/{1}", ServerName, InstanceName);
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

		#endregion Члены ISqlServerInstanceRepository

		private List<SqlServerInstance> GetServerInstances(IList<string> serverNames) {
#if ServerInstancesMock
			return GetServerInstancesMock();
#endif
			var table = InstanceDataTable.Copy();
			var snColumn = table.Columns["ServerName"];
			var inColumn = table.Columns["InstanceName"];
			var vColumn = table.Columns["Version"];
			var res = table.Select("len(ServerName)>0 AND len(InstanceName)>0")
						   .Where(r => serverNames == null || serverNames.Contains(r[snColumn]))
						   .Select(row => new SqlServerInstance {
							   ServerName = (string)row[snColumn],
							   InstanceName = (string)row[inColumn],
							   Version = (string)row[vColumn]
						   }).ToList();
			return res.Count > 0 ? res : GetServerInstancesMock();
		}

		private List<SqlServerInstance> GetServerInstancesMock() {
			return JsonConvert.DeserializeObject<List<SqlServerInstance>>("[{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2005\",\"Name\":\"TSCDISTR-MS/MSSQL2005\",\"Version\":\"9.00.5000.00\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2008\",\"Name\":\"TSCDISTR-MS/MSSQL2008\",\"Version\":\"10.0.5500.0\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2008R2\",\"Name\":\"TSCDISTR-MS/MSSQL2008R2\",\"Version\":\"10.50.2500.0\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2012\",\"Name\":\"TSCDISTR-MS/MSSQL2012\",\"Version\":\"11.0.5058.0\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQL2014\",\"Name\":\"TSCDISTR-MS/MSSQL2014\",\"Version\":\"12.0.2000.8\"},{\"ServerName\":\"TSCDISTR-MS\",\"InstanceName\":\"MSSQLEXPRESS2014\",\"Name\":\"TSCDISTR-MS/MSSQLEXPRESS2014\",\"Version\":\"12.0.2000.8\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2005\",\"Name\":\"TSCPSF-MS/MSSQL2005\",\"Version\":\"9.00.5000.00\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2008\",\"Name\":\"TSCPSF-MS/MSSQL2008\",\"Version\":\"10.0.5500.0\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2008R2\",\"Name\":\"TSCPSF-MS/MSSQL2008R2\",\"Version\":\"10.50.2500.0\"},{\"ServerName\":\"TSCPSF-MS\",\"InstanceName\":\"MSSQL2012\",\"Name\":\"TSCPSF-MS/MSSQL2012\",\"Version\":\"11.0.5058.0\"}]");
		}
	}
}