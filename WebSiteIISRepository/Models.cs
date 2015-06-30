using System.Collections.Generic;
using System.Runtime.Serialization;
using IISAdmin.Interfaces;
using IISAdmin.WCFWebSiteRepository.WebSiteRepositoryService;

namespace IISAdmin.WCFWebSiteRepository
{
	[DataContract]
	public class IisSite : ISite
	{
		#region Members

		public IisSite() {
			Applications = new List<IApplication>();
		}

		public IisSite(Site site) : this() {
			Id = site.Id;
			Name = site.Name;
			State = site.State;
			Bindings = site.Bindings;
			Redis = new RedisInfo(site.Redis);
			Applications.AddRange(site.Applications.ConvertAll(a => new WebApplication(a)));
		}

		public override string ToString() {
			return string.Format("Name: {0}, Applications: [{1}]", Name, string.Join(", ", Applications));
		}

		#endregion Members

		#region ISite Members

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string State { get; set; }

		[DataMember]
		public IRedis Redis { get; set; }

		[DataMember]
		public List<string> Bindings { get; set; }

		[DataMember]
		public List<IApplication> Applications { get; set; }

		#endregion ISite Members
	}

	[DataContract]
	public class WebApplication : IApplication
	{
		#region Members

		public WebApplication(Application application) {
			Name = application.Path;
			Pool = new IisAppPool(application.Pool);
		}

		public override string ToString() {
			return string.Format("Name: {0}, Pool: {1}", Name, Pool.Name);
		}

		#endregion Members

		#region IApplication Members

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public IAppPool Pool { get; set; }

		#endregion IApplication Members
	}

	[DataContract]
	public class IisAppPool : IAppPool
	{
		#region Members

		public IisAppPool(AppPool pool) {
			Name = pool.Name;
			State = pool.State;
		}

		#endregion Members

		#region IAppPool Members

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string State { get; set; }

		#endregion IAppPool Members
	}

	[DataContract]
	public class RedisInfo : IRedis
	{
		public RedisInfo(Redis redis) {
			if (redis == null) {
				return;
			}
			Db = redis.Db;
			Host = redis.Host;
			Port = redis.Port;
			ConnectionString = redis.ConnectionString;
		}

		[DataMember]
		public int Db { get; set; }

		[DataMember]
		public string Host { get; set; }

		[DataMember]
		public int Port { get; set; }

		[DataMember]
		public string ConnectionString { get; set; }
	}
}