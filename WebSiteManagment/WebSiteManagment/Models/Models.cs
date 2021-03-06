﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebSiteManagment.Core.Common;
using MsAdmin = Microsoft.Web.Administration;

namespace WebSiteManagment.Core.Models
{
    [DataContract]
    public class Site
    {
        public Site(long id)
        {
            Id = id;
        }

        public Site(MsAdmin.Site site, IList<MsAdmin.ApplicationPool> pools)
            : this(site.Id)
        {
            Name = site.Name;
            State = site.State.ToString();
            DbConnectionString = WebConfigUtils.GetConnectionString(site, "db");
            var redisCs = WebConfigUtils.GetConnectionString(site, "redis");
            Redis = string.IsNullOrWhiteSpace(redisCs) ? null : new Redis(redisCs);
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
        public List<string> Bindings
        {
            get;
            set;
        }

        [DataMember]
        public Redis Redis { get; set; }

        [DataMember]
        public List<Application> Applications { get; set; }

        [DataMember]
        public string DbConnectionString { get; set; }
    }

    [DataContract]
    public class Application
    {
        public Application(MsAdmin.Application application, MsAdmin.ApplicationPool pool)
        {
            Path = application.Path;
            Pool = new AppPool(pool);
        }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public AppPool Pool
        {
            get;
            set;
        }
    }

    [DataContract]
    public class AppPool
    {
        public AppPool(MsAdmin.ApplicationPool pool)
        {
            Name = pool.Name;
            State = pool.State.ToString();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string State { get; set; }
    }

    [DataContract]
    public class Redis
    {
        private string _connectionString;

        public Redis(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
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
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                var data = SplitString(value);
                Db = GetInt(data, "db");
                Host = GetString(data);
                Port = GetInt(data, "port");
                _connectionString = value;
            }
        }

        private int GetInt(Dictionary<string, string> conectionData, string dataname)
        {
            string num;
            int res = 0;
            var success = conectionData.TryGetValue(dataname, out num) && int.TryParse(num, out res);
            return res;
        }

        private string GetString(Dictionary<string, string> conectionData)
        {
            string res;
            conectionData.TryGetValue("host", out res);
            return res;
        }

        private Dictionary<string, string> SplitString(string conectionString)
        {
            var res = new Dictionary<string, string>();
            var csParams = conectionString.Split(';');
            for (int i = 0; i < csParams.Length; i++) {
                var item = csParams[i];
                var paramInfo = item.Split(new[] {'='}, 2);
                var paramName = paramInfo.Length > 0 ? paramInfo[0] : $"UnknownParameter_{i}";
                var paramValue = paramInfo.Length > 1 ? paramInfo[1] : $"UnknownValue_{i}";
                res[paramName.Trim()] = paramValue.Trim();
            }
            return res;
        }
    }

    public class SiteSettings
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }

    [DataContract]
    public class SiteInfo
    {
        [DataMember]
        public string WebAppDir
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public int AppCount
        {
            get;
            set;
        }

        [DataMember]
        public int? Port
        {
            get;
            set;
        }

        [DataMember]
        public string SiteName
        {
            get;
            set;
        }

        [DataMember]
        public bool CreateNewSite
        {
            get;
            set;
        }
    }
}