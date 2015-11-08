using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace IISAdmin.Interfaces {
    public class Site : IEntity<long> {
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
        public List<Application> Applications { get; set; }

        #region IEntity<long> Members

        [DataMember]
        public long Id { get; set; }

        #endregion

        public override string ToString() {
            return $"Name: {Name}, Applications: [{string.Join(", ", Applications)}]";
        }
    }

    public class Application {
        [DataMember]
        public string SiteName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ApplicationPool Pool { get; set; }

        [DataMember]
        public Redis Redis { get; set; }

        [DataMember]
        public string DbConnectionString { get; set; }

        public override string ToString() {
            return $"Name: {Name}, Pool: {Pool.Name}";
        }
    }

    public class ApplicationPool {
        public string Name { get; set; }
        public string State { get; set; }
    }

    [DataContract]
    public class Redis {
        private readonly Regex _dbRe = new Regex("db=(.*?);");
        private readonly Regex _hostRe = new Regex("host=(.*?);");
        private readonly Regex _portRe = new Regex("port=(.*?);");

        public Redis() {
        }

        public Redis(string connectionString) {
            Contract.Requires(string.IsNullOrEmpty(connectionString));
            ConnectionString = connectionString;
        }

        [DataMember]
        public virtual int Db { get; set; }

        [DataMember]
        public virtual string Host { get; set; }

        [DataMember]
        public virtual int Port { get; set; }

        [DataMember]
        public string ConnectionString {
            get {
                return string.Format("host={0};port={1};db={2};maxReadPoolSize=25;maxWritePoolSize=25", Host, Port, Db);
            }
            set {
                Host = GetFirstMatchGroup(_hostRe, value, "localhost");
                int port, db;
                Port = int.TryParse(GetFirstMatchGroup(_portRe, value, "0"), out port) ? port : 0;
                Db = int.TryParse(GetFirstMatchGroup(_dbRe, value, "0"), out db) ? db : 0;
            }
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