using System;
using System.Collections.Generic;

namespace IISAdmin.Interfaces
{
	#region IWebSiteRepository

	public interface IWebSiteRepository
	{
		List<ISite> GetAllSites();

		ISite GetSite(long id);

		ISite FindSite(long id);

		void StopSite(long siteId);

		void StartSite(long siteId);

		void RestartSitePool(long siteId);

		void FlushSiteRedis(long siteId);

		void SetCacheUsage(bool type);

		void ClearSiteCache();

		long CreateSite(ISiteCreateData data, int appCount);

		void ModifyConnectionStrings(long siteId, Dictionary<string, string> config);

		int GetFreePortNumber();

	}

	#endregion IWebSiteRepository

	#region IReleaseRepository

	public interface IRelease : IEntity<Guid>
	{
		string Name { get; set; }

		string Version { get; set; }

		bool Release { get; set; }
				
		DateTime CreatedOn { get; set; }

		string ZipFilePath { get; }
	}

	public interface IReleaseRepository : IRepository<IRelease, Guid>
	{
		IRelease GetByUri(string uri);
		IEnumerable<IRelease> GetTopThousand(string nameLike);
	}

	#endregion IReleaseRepository

	#region ISqlServerInstanceRepository

	public interface ISqlServerInstance
	{
		string ServerName { get; set; }

		string InstanceName { get; set; }

		string Name { get; }

		string Version { get; set; }
	}

	public interface ISqlServerInstanceRepository
	{
		IList<ISqlServerInstance> GetAllInstances(IList<string> serverNameFilter = null);
	}

	public class SqlServerInstanceComparer:IEqualityComparer<ISqlServerInstance> {

		public static readonly SqlServerInstanceComparer Instance = new SqlServerInstanceComparer();

		public bool Equals(ISqlServerInstance x, ISqlServerInstance y) {
			return CompareNames (x,y) == 0;
		}

		private int CompareNames(ISqlServerInstance x, ISqlServerInstance y) {
			return string.Compare(x.ServerName, y.ServerName, StringComparison.OrdinalIgnoreCase)
				+ string.Compare(x.InstanceName, y.InstanceName, StringComparison.OrdinalIgnoreCase)
				+ string.Compare(x.Version, y.Version, StringComparison.OrdinalIgnoreCase);
		}

		public int GetHashCode(ISqlServerInstance obj) {
			return obj.ServerName.GetHashCode() + obj.InstanceName.GetHashCode() + obj.Version.GetHashCode();
		}
	}
	#endregion ISqlServerInstanceRepository
	
}