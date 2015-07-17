using System;
using System.Collections.Generic;
using System.Threading;
using IISAdmin.Interfaces;
using IISAdmin.WebSiteManagmentProvider;
using Moq;

namespace IISAdmin.Owin.Mock
{
	public class SiteDeployProviderMock:ISiteDeployProvider
    {
		private ISiteDeployProvider _realDeployProvider;

		public SiteDeployProviderMock(ISiteDeployProviderWebConfig config) {
			var repoMock = new Mock<IWebSiteRepository>();
			_realDeployProvider = new SiteDeployProvider(config, repoMock.Object);
		}
		public void DeployWebApp(ISiteCreateData siteCreateData, ISiteDeployProgress progressInfo) {
			progressInfo.SetNextOperation();
			InitDeployInfo(siteCreateData);
			var wait = 10;
			Thread.Sleep(TimeSpan.FromSeconds(wait));
			//ExtractBinaries(siteCreateData);
			progressInfo.SetNextOperation();
			//ModifyConnectionStrings
		}

		public ISiteDeployProgress GetInitDeployProgress(IEnumerable<DeployOperationIfo> extraOperations) {
			return _realDeployProvider.GetInitDeployProgress(extraOperations);
		}

		public void InitDeployInfo(ISiteCreateData siteCreateData) {
			_realDeployProvider.InitDeployInfo(siteCreateData);
		}
	}
}
