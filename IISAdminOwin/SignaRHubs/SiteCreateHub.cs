using System.Collections.Generic;
using System.Linq;
using IISAdmin.Interfaces;
using IISAdmin.Owin.Models;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs
{
	public class SiteCreate : BaseHub<SiteManagement>
	{
		private IWebSiteRepository _siteRepository;
		private IReleaseRepository _releaseRepository;
		private ISqlServerInstanceRepository _serverInstanceRepository;

		public SiteCreate(IWebSiteRepository siteRepository, IReleaseRepository releaseRepository, ISqlServerInstanceRepository serverInstanceRepository)
		{
			_siteRepository = siteRepository;
			_releaseRepository = releaseRepository;
			_serverInstanceRepository = serverInstanceRepository;
		}

		public IRelease GetReleaseInfo(string uri)
		{
			return _releaseRepository.GetByUri(uri);
		}

		public SiteCreationInfo GetStartupInfo()
		{
			var sqlInstances = _serverInstanceRepository.GetAllInstances();
			var res = new SiteCreationInfo
			{
				FreeRedisDbNum = GetFreeRedisDb(),
				SqlServerInstances = sqlInstances
			};
			return res;
		}

		private int GetFreeRedisDb()
		{
			_siteRepository.ClearSiteCache();
			var sites = _siteRepository.GetAllSites();
			var numberFound = false;
			var number = -1;
			do
			{
				number++;
				if (sites.Any(s => s.Redis.Db == number)) continue;
				numberFound = true;
			} while (!numberFound);
			return number;
		}

		public void AddSite(SiteCreateData data)
		{
			_siteRepository.CreateSite(data);
		}
	}

	[JsonObject]
	public class SiteCreationInfo
	{
		public int FreeRedisDbNum { get; set; }

		public IList<ISqlServerInstance> SqlServerInstances { get; set; }
	}

	[JsonObject]
	public class SiteCreateData : ISiteCreateData
	{
		public string Name { get; set; }

		public int Redis { get; set; }

		public SignalrRelease ReleaseInfo { get; set; }

		public SignalrSqlServerInstance Db { get; set; }

		#region Члены ISiteCreateData

		IRelease ISiteCreateData.ReleaseInfo
		{ get { return ReleaseInfo; } }

		ISqlServerInstance ISiteCreateData.Db
		{ get { return Db; } }

		#endregion Члены ISiteCreateData
	}
}