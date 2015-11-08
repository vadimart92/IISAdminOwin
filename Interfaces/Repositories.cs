using System;
using System.Collections.Generic;
using System.Linq;

namespace IISAdmin.Interfaces
{
	#region IWebSiteRepository

	public interface IWebSiteRepository
	{
		List<Site> GetAllSites();

		List<Application> GetAllApplications();

		Site GetSite(long id);

		Site FindSite(long id);

		void StopSite(long siteId);

		void StartSite(long siteId);

		void RestartSitePool(long siteId);

		void FlushSiteRedis(long siteId);

		void SetCacheUsage(bool type);

		void ClearSiteCache();

		long CreateSite(SiteCreateData data, int appCount);

		void ModifyConnectionStrings(long siteId, Dictionary<string, string> config);

		int GetFreePortNumber();

	}

	#endregion IWebSiteRepository

	#region IReleaseRepository

    [Serializable]
    public class Release : IEntity<Guid> {
        public virtual string Name { get; set; }
        public virtual string Version { get; set; }
        public virtual bool IsRelease { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual string ZipFilePath { get; set; }
        public virtual Guid Id { get; set; }
    }

    public interface IReleaseRepository : IRepository<Release, Guid>
	{
		Release GetByUri(string uri);
		IEnumerable<Release> GetTopThousand(string nameLike);
	}

    #endregion IReleaseRepository

    #region ISqlServerInstanceRepository

    [Serializable]
    public class SqlServerInstance {
        public string ServerName { get; set; }
        public string InstanceName { get; set; }
        public string Version { get; set; }
        public string Name
        {
            get
            {
                return (new List<string> { ServerName, InstanceName }).Where(s => !string.IsNullOrWhiteSpace(s)).Aggregate((a, b) => a + "/" + b);
            }
        }
    }

    public interface ISqlServerInstanceRepository
	{
		IList<SqlServerInstance> GetAllInstances(IList<string> serverNameFilter = null);
	}

	public class SqlServerInstanceComparer:IEqualityComparer<SqlServerInstance> {

		public static readonly SqlServerInstanceComparer Instance = new SqlServerInstanceComparer();

		public bool Equals(SqlServerInstance x, SqlServerInstance y) {
			return CompareNames (x,y) == 0;
		}

		private int CompareNames(SqlServerInstance x, SqlServerInstance y) {
			return string.Compare(x.ServerName, y.ServerName, StringComparison.OrdinalIgnoreCase)
				+ string.Compare(x.InstanceName, y.InstanceName, StringComparison.OrdinalIgnoreCase)
				+ string.Compare(x.Version, y.Version, StringComparison.OrdinalIgnoreCase);
		}

		public int GetHashCode(SqlServerInstance obj) {
			return obj.ServerName.GetHashCode() + obj.InstanceName.GetHashCode() + obj.Version.GetHashCode();
		}
	}
	#endregion ISqlServerInstanceRepository
	
}