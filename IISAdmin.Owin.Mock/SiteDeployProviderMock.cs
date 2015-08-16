using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using IISAdmin.Interfaces;
using IISAdmin.WebSiteManagmentProvider;
using Moq;

namespace IISAdmin.Owin.Mock
{
	public class SiteDeployProviderMock:ISiteDeployProvider
    {
		private readonly ISiteDeployProvider _realDeployProvider;
		private readonly IJobInfoRepository _jobInfoRepository;
		private readonly IHubContextProvider _hubContextProvider;

		public SiteDeployProviderMock(ISiteDeployProviderWebConfig config, IJobInfoRepository jobInfoRepository, IHubContextProvider hubContextProvider) {
		    _jobInfoRepository = jobInfoRepository;
			var repoMock = new Mock<IWebSiteRepository>();
			_realDeployProvider = new SiteDeployProvider(config, repoMock.Object, jobInfoRepository, hubContextProvider);
		    _hubContextProvider = hubContextProvider;
		}
		public void DeployWebApp(SiteCreateData siteCreateData, Guid jobInfoId) {
            var deployInfo = new DeploySiteInfo(_jobInfoRepository, _hubContextProvider, jobInfoId);
            InitDeployInfo(siteCreateData);
			var wait = 3;
			Thread.Sleep(TimeSpan.FromSeconds(wait));
            //ExtractBinaries(siteCreateData);
            deployInfo.CreateWebApp = OperationStageState.Completed;
            Thread.Sleep(TimeSpan.FromSeconds(wait));
            //ModifyConnectionStrings
            deployInfo.RestoreDbCopyFiles = OperationStageState.Completed;
        }
        
	    public void InitDeployInfo(SiteCreateData siteCreateData) {
			_realDeployProvider.InitDeployInfo(siteCreateData);
		}

	    public OperationInfoBase GetOperationsInfo() {
           return _realDeployProvider.GetOperationsInfo();
        }
        
	}
}
