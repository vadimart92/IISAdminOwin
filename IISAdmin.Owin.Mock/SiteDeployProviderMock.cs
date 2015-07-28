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

		public SiteDeployProviderMock(ISiteDeployProviderWebConfig config) {
			var repoMock = new Mock<IWebSiteRepository>();
			_realDeployProvider = new SiteDeployProvider(config, repoMock.Object);
		}
		public void DeployWebApp(ISiteCreateData siteCreateData, OperationInfoBase deploySiteInfo) {
            var deployInfo = deploySiteInfo as DeploySiteInfo;
            Debug.Assert(deployInfo != null, "deployInfo != null");
            deployInfo.RestoreDbCopyFiles = OperationStageState.Completed;
			InitDeployInfo(siteCreateData);
			var wait = 3;
			Thread.Sleep(TimeSpan.FromSeconds(wait));
            //ExtractBinaries(siteCreateData);
            deployInfo.CreateWebApp = OperationStageState.Completed;
            Thread.Sleep(TimeSpan.FromSeconds(wait));
            //ModifyConnectionStrings
            deployInfo.RestoreDbCopyFiles = OperationStageState.Completed;
        }
        
		public void InitDeployInfo(ISiteCreateData siteCreateData) {
			_realDeployProvider.InitDeployInfo(siteCreateData);
		}

	    public OperationInfoBase GetOperationsInfo(IJobInfoRepository jobInfoRepository) {
	        return _realDeployProvider.GetOperationsInfo(jobInfoRepository);
	    }
	}
}
