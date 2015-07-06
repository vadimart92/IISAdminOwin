using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IISAdmin.WebSiteManagmentProvider.Test {
	[TestClass]
	public class SiteDeployProviderTest {
		[TestMethod]
		public void RestoreBD_DbExists() {
			var deployProvider = new SiteDeployProvider();
			deployProvider.DeployWebApp();
		}
	}
}
