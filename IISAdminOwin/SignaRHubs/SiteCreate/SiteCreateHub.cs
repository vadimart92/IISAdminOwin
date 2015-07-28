using System;
using System.Diagnostics.Contracts;
using IISAdmin.Interfaces;
using IISAdmin.Owin.Models;
using IISAdmin.WebSiteManagmentProvider;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs.SiteCreate {
	public class SiteCreateHub : BaseHub<SiteCreateHub> {
		private readonly IReleaseRepository _releaseRepository;
		private readonly ISqlServerInstanceRepository _serverInstanceRepository;
		private readonly ISiteDeployProvider _siteDeployProvider;
	    private readonly IJobInfoRepository _jobInfoRepository;

		public SiteCreateHub(IReleaseRepository releaseRepository, ISqlServerInstanceRepository serverInstanceRepository, 
			ISiteDeployProvider siteDeployProvider, IJobInfoRepository jobInfoRepository) {
			Contract.Assert(releaseRepository != null);
			Contract.Assert(serverInstanceRepository != null);
			Contract.Assert(siteDeployProvider != null);
			Contract.Assert(jobInfoRepository != null);
			_releaseRepository = releaseRepository;
			_serverInstanceRepository = serverInstanceRepository;
			_siteDeployProvider = siteDeployProvider;
		    _jobInfoRepository = jobInfoRepository;
		}

		public ReleaseInfo GetReleaseInfo(string uri) {
			var result = _releaseRepository.GetByUri(uri);
			var siteCreateData = new SiteCreateData {
				ReleaseInfo = new SignalrRelease(result),
				Name = result.Name,
				UserName = Context.User.Identity.Name
			};
			_siteDeployProvider.InitDeployInfo(siteCreateData);
			return new ReleaseInfo {
				Release = result,
				WebAppName = siteCreateData.WebAppName,
				WebAppDir = siteCreateData.DestinationWebAppRoot
			};
		}

		public SiteCreationInfo GetStartupInfo() {
			var sqlInstances = _serverInstanceRepository.GetAllInstances();
			var res = new SiteCreationInfo {
				SqlServerInstances = sqlInstances
			};
			return res;
		}
		
		public void AddSite(SiteCreateData data) {
			data.UserName = Context.User.Identity.Name;
		    var progressInfo = _siteDeployProvider.GetOperationsInfo(_jobInfoRepository);
		    progressInfo.OnStateChanged += info => {
                Clients.All.updateSiteState(JsonConvert.SerializeObject(info));
            };
			_siteDeployProvider.DeployWebApp(data, progressInfo);
		}
	}
}