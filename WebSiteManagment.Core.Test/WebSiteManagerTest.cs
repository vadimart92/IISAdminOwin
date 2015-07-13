using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSiteManagment.Core.Models;

namespace WebSiteManagment.Core.Test {
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
			Site site;
			try {
				_mgr.AddSite(info);
				site = _mgr.GetWebsites().FirstOrDefault(s => s.Name == _webAppName);
			}
			finally {
				_mgr.RemoveSite(_webAppName, true, true);
			}
			Assert.IsNotNull(site);
		}
	}
}
