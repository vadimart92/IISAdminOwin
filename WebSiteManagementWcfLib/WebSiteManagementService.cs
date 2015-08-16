using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WebSiteManagment.Core;
using WebSiteManagment.Core.Common;
using WebSiteManagment.Core.Models;

namespace WebSiteManagment.Wcf
{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	public class WebSiteManagementService : IWebSiteRepositoryService
	{
		private readonly WebSiteManager _siteManager = new WebSiteManager();

		public List<Site> GetAllSites() {
			try {
				var sites = _siteManager.GetWebsites();
				return sites;
			} catch (UnauthorizedAccessException ex) {
				throw new FaultException<UnauthorizedAccessException>(ex, "AccessDenied");
			}
		}

		public Site GetSite(long id) {
			var site = _siteManager.GetSite(id);
			return site;
		}

		public void StartSite(long id) {
			_siteManager.StartSite(id);
		}

		public void RestartSitePool(long siteId) {
			var site = _siteManager.GetSite(siteId);
		    var app = site.Applications[0];
            _siteManager.RestartPool(app.Pool.Name);
		}

		public void FlushSiteRedis(long siteId) {
			_siteManager.FlushRedis(siteId);
		}

		public void SetCacheUsage(bool type) {
			_siteManager.SetCasheUsage(type);
		}

		public void ClearCache() {
			_siteManager.ClearCashe();
		}
		
		public long AddSite(SiteInfo information) {
			return _siteManager.AddSite(information);
		}

		public void ModifyConnectionStrings(long siteName, Dictionary<string, string> config) {
			_siteManager.ModifyConnectionStrings(siteName, config);
		}

		public int GetNexFreePort() {
			return CommonUtils.SafeGetFreeTcpPort();
		}

		public Site FindSite(long id) {
			var site = _siteManager.FindSite(id);
			return site;
		}

		public void StopSite(long id) {
			_siteManager.StopSite(id);
		}
	}
}