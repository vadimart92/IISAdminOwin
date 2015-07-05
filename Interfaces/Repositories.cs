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

		void CreateSite(ISiteCreateData data);
	}

	#endregion IWebSiteRepository

	#region IReleaseRepository

	public interface IRelease : IEntity<Guid>
	{
		string Name { get; set; }

		string Version { get; set; }

		bool Release { get; set; }

		string BuildFolderLink { get; set; }
				
		DateTime CreatedOn { get; set; }

		string ZipFilePath { get; }
	}

	public interface IReleaseRepository : IRepository<IRelease, Guid>
	{
		IRelease GetByUri(string uri);
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

	#endregion ISqlServerInstanceRepository
	
}