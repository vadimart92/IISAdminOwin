using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.SignaRHubs {
	
	public class SiteManagement: BaseHub<SiteManagement> {
	
		private IWebSiteRepository _siteRepository;
		
		public SiteManagement(IWebSiteRepository siteRepository) {
			_siteRepository = siteRepository;
		}

		public void RestartPool(long siteId) {
			ExecuteSiteOperation(siteId, (id) => {
				UpdateSiteState(siteId, new Dictionary<string, object> { { "poolRestartState", OperationState.Restarting } });
				_siteRepository.RestartSitePool(id);
				Thread.Sleep(TimeSpan.FromSeconds(1));
				var site = _siteRepository.GetSite(siteId);
				return new SiteDataClass {
					Name = site.Applications.First().Pool.Name,
					Data = new Dictionary<string, object> {
						{"poolRestartState", OperationState.Running}
					}
				};
			}, "Pool {0} restarted.", "While restarting pool {0}.");
		}

		public void FlushRedis(long siteId) {
			ExecuteSiteOperation(siteId, (id) => {
				_siteRepository.FlushSiteRedis(id);
				var site = _siteRepository.GetSite(siteId);
				return new SiteDataClass {
					Name = site.Applications.First().Pool.Name
				};
			}, "Redis for site {0} flushed.", "While flushing redis for site {0}.");
		}

		public void StopSite(long siteId) {
			ExecuteSiteOperation(siteId, (id) => {
				_siteRepository.StopSite(id);
				return StopOrStartSite(id);
			}, "Site {0} stopped.", "While stopping site {0}.");
		}

		private SiteDataClass StopOrStartSite(long id) {
			var site = _siteRepository.GetSite(id);
			return new SiteDataClass {
				Name = site.Name,
				Data = new Dictionary<string, object> {
						{"state", site.State}
					}
			};
		}
		
		public void StartSite(long siteId) {
			ExecuteSiteOperation(siteId, (id) => {
				_siteRepository.StartSite(id);
				return StopOrStartSite(id);
			}, "Site {0} started.", "While starting site {0}.");
		}

		private void ExecuteSiteOperation(long siteId, Func<long, SiteDataClass> operationAction, string succesMsgTpl, string errMsgTpl) {
			var siteName = string.Format("site id: {0}", siteId);
			try {
				var data = operationAction(siteId);
				siteName = data.Name;
				Information.SuccessFormat(Context.User, "Done", succesMsgTpl, siteName);
				UpdateSiteState(siteId, data.Data);
			} catch {
				Information.ErrorFormat(Context.User, "Error", errMsgTpl, siteName);
			}
		}

		public void UpdateSiteState(long siteId, Dictionary<string, object> newData) {
			Clients.All.updateSiteState(new SiteOperationState {
				OperationTime = DateTime.UtcNow,
				Id = siteId,
				NewData = newData
			});
		}

		private class SiteDataClass {
			public SiteDataClass() {
				Data = new Dictionary<string, object>();
			}
			public string Name { get; set; }
			public Dictionary<string, object> Data { get; set; }
		}
	}
}