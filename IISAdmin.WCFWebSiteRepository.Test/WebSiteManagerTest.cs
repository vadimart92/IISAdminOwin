using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using IISAdmin.Owin.SignaRHubs.SiteCreate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSiteManagment.Core;
using WebSiteManagment.Core.Models;
using WebSiteManagment.Wcf;

namespace IISAdmin.WCFWebSiteRepository.Test {
	[TestClass]
	public class WebSiteManagerTest {

		private string _testSitePath;
		private string _testWebAppPath;
		private readonly string _webAppName = "testWebApp";
		WebSiteManager _mgr = new WebSiteManager();
		[TestInitialize]
		public void Init() {
			var di = new DirectoryInfo(Environment.CurrentDirectory);
			_testSitePath = Path.Combine(di.Root.Name, _webAppName);
			_testWebAppPath = Path.Combine(_testSitePath, "Terrasoft.WebApp");
			Directory.CreateDirectory(_testSitePath);
			Directory.CreateDirectory(_testWebAppPath);
		}
		
		[TestCleanup]
		public void Cleanup() {
			Directory.Delete(_testSitePath, true);
		}

		[TestMethod]
		public void AddSite() {
			var info = new SiteInfo() {
				Name = _webAppName,
				Port = 100,
				WebAppDir = _testSitePath,
				SiteName = _webAppName,
				AppCount = 2
			};
			_mgr.AddSite(info);
			var site = _mgr.GetWebsites().FirstOrDefault(s => s.Name == _webAppName);
			_mgr.RemoveSite(_webAppName, true, true);
			Assert.IsNotNull(site);
		}
	}
}
