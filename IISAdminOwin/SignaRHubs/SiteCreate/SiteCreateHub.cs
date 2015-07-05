using System;
using System.Diagnostics.Contracts;
using System.Linq;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.SignaRHubs.SiteCreate {
	public class SiteCreateHub : BaseHub<SiteCreateHub> {
		private readonly IReleaseRepository _releaseRepository;
		private readonly ISqlServerInstanceRepository _serverInstanceRepository;
		private readonly ISiteDeployProvider _siteDeployProvider;
		private readonly IWebSiteRepository _siteRepository;

		public SiteCreateHub(IWebSiteRepository siteRepository, IReleaseRepository releaseRepository,
			ISqlServerInstanceRepository serverInstanceRepository, ISiteDeployProvider siteDeployProvider) {
			Contract.Assert(siteRepository != null);
			Contract.Assert(releaseRepository != null);
			Contract.Assert(serverInstanceRepository != null);
			Contract.Assert(siteDeployProvider != null);
			_siteRepository = siteRepository;
			_releaseRepository = releaseRepository;
			_serverInstanceRepository = serverInstanceRepository;
			_siteDeployProvider = siteDeployProvider;
		}

		public IRelease GetReleaseInfo(string uri) {
			return _releaseRepository.GetByUri(uri);
		}

		public SiteCreationInfo GetStartupInfo() {
			var sqlInstances = _serverInstanceRepository.GetAllInstances();
			var res = new SiteCreationInfo {
				FreeRedisDbNum = GetFreeRedisDb(),
				SqlServerInstances = sqlInstances
			};
			return res;
		}

		private int GetFreeRedisDb() {
			_siteRepository.ClearSiteCache();
			var sites = _siteRepository.GetAllSites();
			var numberFound = false;
			var number = -1;
			do {
				number++;
				if (sites.Any(s => s.Redis.Db == number)) continue;
				numberFound = true;
			} while (!numberFound);
			return number;
		}

		public void AddSite(SiteCreateData data) {
			var progress = new Progress<ISiteDeployProgress>(info => { Clients.All.updateSiteState(info); });
			var progressInfo = _siteDeployProvider.GetInitDeployProgress(new[] { new DeployOperationIfo { Info = "Creating Pool/WebApp in IIS" } });
			_siteDeployProvider.DeployWebApp(data, progress, ref progressInfo);
			progressInfo.SetNextOperation();
			((IProgress<ISiteDeployProgress>)progress).Report(progressInfo);
			_siteRepository.CreateSite(data);
		}
	}
}