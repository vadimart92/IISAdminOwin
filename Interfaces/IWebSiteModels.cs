using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace IISAdmin.Interfaces
{
	public interface ISite : IEntity<long>
	{
		string Name { get; set; }

		string State { get; set; }
        
		List<string> Bindings { get; set; }

		List<IApplication> Applications { get; set; }
    }

	public interface IApplication {

	    string SiteName { get; set; }

        string Name { get; set; }

		IAppPool Pool { get; set; }

        Redis Redis { get; set; }

        string DbConnectionString { get; set; }
    }

	public interface IAppPool
	{
		string Name { get; set; }

		string State { get; set; }
	}

	[DataContract]
	public class Redis {

		private Regex _hostRe = new Regex("host=(.*?);");
		private Regex _portRe = new Regex("port=(.*?);");
		private Regex _dbRe = new Regex("db=(.*?);");

		[DataMember]
		public virtual int Db { get; set; }
		
		[DataMember]
		public virtual  string Host { get; set; }
		
		[DataMember]
		public virtual  int Port { get; set; }

		[DataMember]
		public  string ConnectionString
		{
			get { return string.Format("host={0};port={1};db={2};maxReadPoolSize=25;maxWritePoolSize=25", Host, Port, Db); }
			set {
				Host = GetFirstMatchGroup(_hostRe, value, "localhost");
				int port, db;
				Port = Int32.TryParse(GetFirstMatchGroup(_portRe, value, "0"), out port) ? port : 0;
				Db = Int32.TryParse(GetFirstMatchGroup(_dbRe, value, "0"), out db)? db : 0;
			}
		}

		public Redis() { }

		public Redis(string connectionString) {
			Contract.Requires(string.IsNullOrEmpty(connectionString));
			ConnectionString = connectionString;
		}

		private string GetFirstMatchGroup(Regex re, string value, string defValue) {
			var m = re.Match(value);
			if (m.Success && m.Groups.Count > 0) {
				return m.Groups[1].Value;
			}
			return defValue;
		}
	}
}