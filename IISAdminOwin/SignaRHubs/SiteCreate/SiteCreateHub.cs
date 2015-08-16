using System;
using System.Diagnostics.Contracts;
using IISAdmin.Interfaces;
using IISAdmin.WebSiteManagmentProvider;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs.SiteCreate {
	public class SiteCreateHub : BaseHub<SiteCreateHub> {
		private readonly IReleaseRepository _releaseRepository;
		private readonly ISqlServerInstanceRepository _serverInstanceRepository;
		private readonly ISiteDeployProvider _siteDeployProvider;
	    private readonly IJobInfoRepository _jobInfoRepository;
	    private readonly IBackgroundWorker _backgroundWorker;

		public SiteCreateHub(IReleaseRepository releaseRepository, ISqlServerInstanceRepository serverInstanceRepository, 
			ISiteDeployProvider siteDeployProvider, IJobInfoRepository jobInfoRepository, IBackgroundWorker backgroundWorker) {
			Contract.Assert(releaseRepository != null);
			Contract.Assert(serverInstanceRepository != null);
			Contract.Assert(siteDeployProvider != null);
			Contract.Assert(jobInfoRepository != null);
			Contract.Assert(backgroundWorker != null);
			_releaseRepository = releaseRepository;
			_serverInstanceRepository = serverInstanceRepository;
			_siteDeployProvider = siteDeployProvider;
		    _jobInfoRepository = jobInfoRepository;
		    _backgroundWorker = backgroundWorker;
		}

		public ReleaseInfo GetReleaseInfo(string uri) {
			var result = _releaseRepository.GetByUri(uri);
			var siteCreateData = new SiteCreateData {
				ReleaseInfo = result,
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
		    OperationInfoBase progressInfo = _siteDeployProvider.GetOperationsInfo();
		    progressInfo.SignarRHubName = GetType().Name;
		    progressInfo.OnStateChanged += (hub, info) => {
                var dataStr = JsonConvert.SerializeObject(info);
                hub.Clients.All.updateSiteState(dataStr);
            };
		    var progressJobId = progressInfo.Id;
            _backgroundWorker.AddJob<ISiteDeployProvider>(siteDeployProvider => siteDeployProvider.DeployWebApp(data, progressJobId), progressInfo);
		}
	}
}