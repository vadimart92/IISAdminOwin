using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebSiteManagment.Common;
using MsAdmin = Microsoft.Web.Administration;

namespace WebSiteManagment.Models {
	
	[DataContract]
    public class Site {
	    public Site(long id){
		    Id = id;
	    }

		public Site(MsAdmin.Site site, IList<MsAdmin.ApplicationPool> pools)
			: this(site.Id) {
		    Name = site.Name;
			State = site.State.ToString();
			var redisCs = WebConfigUtils.GetRedisConnectionString(site);
			Redis = string.IsNullOrWhiteSpace(redisCs)? null : new Redis(redisCs);
			Bindings = site.Bindings.ToList().ConvertAll(b => b.BindingInformation);
			Applications = site.Applications.ToList().ConvertAll(a => 
				new Application(a, pools.FirstOrDefault(p => 
						string.Equals(p.Name, a.ApplicationPoolName, StringComparison.OrdinalIgnoreCase)
					)
				)
			);
	    }

		[DataMember]
	    public long Id { get; set; }
		
		[DataMember]
        public string Name { get; set; }

		[DataMember]
		public string State { get; set; }

		[DataMember]
		public List<string> Bindings {
			get;
			set;
		}

		[DataMember]
		public Redis Redis { get; set; }

		[DataMember]
		public List<Application> Applications { get; set; }
        
    }

	[DataContract]
    public class Application {
	    public Application(MsAdmin.Application application, MsAdmin.ApplicationPool pool) {
		    Path = application.Path;
		    Pool = new AppPool(pool);
	    }

		[DataMember]
        public string Path { get; set; }

		[DataMember]
		public AppPool Pool {
			get;
			set;
		}
    }

	[DataContract]
    public class AppPool {
	    public AppPool(MsAdmin.ApplicationPool pool) {
		    Name = pool.Name;
		    State = pool.State.ToString();
	    }

		[DataMember]
        public string Name { get; set; }

		[DataMember]
        public string State { get; set; }
    }

	[DataContract]
    public class Redis {
		private string _connectionString;

		public Redis(string connectionString) {
		    if (string.IsNullOrWhiteSpace(connectionString)) {
                throw new ArgumentException("connectionString");
		    }
			ConnectionString = connectionString;
	    }

		[DataMember]
        public int Db { get; set; }

		[DataMember]
        public string Host { get; set; }

		[DataMember]
        public int Port { get; set; }

		[DataMember]
		public string ConnectionString {
			get { return _connectionString; }
			set {
				var data = SplitString(value);
				Db = GetInt(data, "db");
				Host = GetString(data);
				Port = GetInt(data, "port");
				_connectionString = value;
			}
		}

		private int GetInt(Dictionary<string, string> conectionData, string dataname) {
			string num;
			int res = 0;
			var success = conectionData.TryGetValue(dataname, out num) && int.TryParse(num, out res);
			return res;
		}
		private string GetString(Dictionary<string, string> conectionData) {
			string res;
			conectionData.TryGetValue("host", out res);
			return res;
		}

		private Dictionary<string, string> SplitString(string conectionString) {
			return conectionString.Split(';')
			.Select(t => t.Split(new [] { '=' }, 2))
			.ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
		}
	}
}
