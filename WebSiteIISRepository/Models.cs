﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using IISAdmin.Interfaces;
using Redis = IISAdmin.Interfaces.Redis;

namespace IISAdmin.WCFWebSiteRepository
{
	[DataContract]
	public class IisSite : ISite
	{
		#region Members

		public IisSite() {
			Applications = new List<IApplication>();
		}

		public IisSite(WebSiteManagment.Core.Models.Site site)
			: this() {
			Id = site.Id;
			Name = site.Name;
			State = site.State;
			Bindings = site.Bindings;
			Redis = new RedisInfo(site.Redis);
			DbConnectionString = site.DbConnectionString;
			Applications.AddRange(site.Applications.ConvertAll(a => new WebApplication(a, site)));
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
		public Redis Redis { get; set; }

		[DataMember]
		public string DbConnectionString { get; set; }

		[DataMember]
		public List<string> Bindings { get; set; }

		[DataMember]
		public List<IApplication> Applications { get; set; }

		#endregion ISite Members
	}

	[DataContract]
	public class WebApplication : IApplication {

	    public WebApplication(WebSiteManagment.Core.Models.Application application, WebSiteManagment.Core.Models.Site site) {
			Name = application.Path;
			Pool = new IisAppPool(application.Pool);
            SiteName = site.Name;
	        Redis = new RedisInfo(site.Redis);
	        DbConnectionString = site.DbConnectionString;
	    }

		public override string ToString() {
			return string.Format("Name: {0}, Pool: {1}", Name, Pool.Name);
		}

        #region IApplication Members

        [DataMember]
        public string SiteName { get; set; }

	    [DataMember]
		public string Name { get; set; }

		[DataMember]
		public IAppPool Pool { get; set; }

        [DataMember]
        public Redis Redis { get; set; }

        [DataMember]
        public string DbConnectionString { get; set; }

	    #endregion IApplication Members
	}

	[DataContract]
	public class IisAppPool : IAppPool
	{
		#region Members

		public IisAppPool(WebSiteManagment.Core.Models.AppPool pool) {
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
	public class RedisInfo : Redis
	{
		public RedisInfo(WebSiteManagment.Core.Models.Redis redis) {
			if (redis == null) {
				return;
			}
			Db = redis.Db;
			Host = redis.Host;
			Port = redis.Port;
		}

		[DataMember]
		public override int Db { get; set; }

		[DataMember]
		public override string Host { get; set; }

		[DataMember]
		public override int Port { get; set; }
	}
}