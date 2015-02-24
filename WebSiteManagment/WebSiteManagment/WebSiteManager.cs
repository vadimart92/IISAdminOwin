using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using WebSiteManagment.Core.Common;
using WebSiteManagment.Core.Models;
using Site = WebSiteManagment.Core.Models.Site;

namespace WebSiteManagment.Core {
	public class WebSiteManager {
		#region  Members

		public ServerManager _serverManager;
		private static ConcurrentDictionary<long, Site> _siteCache = new ConcurrentDictionary<long, Site>();
		private static readonly object Synchronizer = new object();
		private bool _useCache = true;

		public WebSiteManager() {
			var sys32Dir = Environment.GetFolderPath(Environment.SpecialFolder.System);
			var filePath = sys32Dir + AppHostConfigPath;
			_serverManager = new ServerManager(filePath);
		}

		public void ClearCashe() {
			_siteCache.Clear();
		}

		public void SetCasheUsage(bool type) {
			lock (Synchronizer) {
				_useCache = type;
			}
		}

		public List<Site> GetWebsites() {
			lock (Synchronizer) {
				if (_useCache) {
					if (_serverManager.Sites.Count == _siteCache.Count) {
						return _siteCache.Values.ToList();
					}
					var cached = _siteCache.Values.ToList();
					var newSites = _serverManager.Sites
									.Where(s => cached.All(c => c.Id != s.Id)).ToList()
									.ConvertAll(s => new Site(s, _serverManager.ApplicationPools.ToList()));
					AddSitesToCache(newSites);
					return cached.Concat(newSites).ToList();
				}	
			}
			var sites = _serverManager.Sites.ToList().ConvertAll(s => new Site(s,_serverManager.ApplicationPools.ToList()));
			AddSitesToCache(sites);
			return sites;
		}

		private static void AddSitesToCache(List<Site> sites) {
			Task.Run(() => {
				foreach (var site in sites) {
					_siteCache[site.Id] = site;
				}
			});
		}

		public Site GetSite(long id) {
			var site = FindSite(id);
			if (site != null) {
				return site;
			}
			throw new ArgumentOutOfRangeException("id");
		}

		public Site FindSite(long id) {
			Site res;
			if (_useCache && _siteCache.TryGetValue(id, out res)) {
				return res;
			}
			var site = _serverManager.Sites.FirstOrDefault(s => s.Id == id);
			if (site != null) {
				res = new Site(site, _serverManager.ApplicationPools.ToList());
				if (_useCache) {
					_siteCache[id] = res;
				}
				return res;
			}
			return null;
		}

		public List<AppPool> GetPools() {
			return _serverManager.ApplicationPools.ToList().ConvertAll(p => new AppPool(p));
		}

		public void StopSite(long id) {
			var site = _serverManager.Sites.FirstOrDefault(s => s.Id == id);
			if (site != null) {
				RemoveSiteFromCache(site);
				site.Stop();
			}
		}

		private void RemoveSiteFromCache(Microsoft.Web.Administration.Site site) {
			if (_useCache) {
				Site res;
				_siteCache.TryRemove(site.Id, out res);
			}
		}

		public void StartSite(long id) {
			var site = _serverManager.Sites.FirstOrDefault(s => s.Id == id);
			if (site != null) {
				RemoveSiteFromCache(site);
				site.Start();
			}
		}

		public void StopPool(string name) {
			var pool =
				_serverManager.ApplicationPools.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			if (pool != null) {
				_siteCache.Clear();
				ProcessUtils.StopPool(pool);
			}
		}

		public void StopPool(int index) {
			if (index < _serverManager.ApplicationPools.Count) {
				var pool = _serverManager.ApplicationPools[index];
				_siteCache.Clear();
				ProcessUtils.StopPool(pool);
			}
		}

		public void RestartPool(string poolName) {
			if (string.IsNullOrWhiteSpace(poolName)) {
				throw  new ArgumentOutOfRangeException("poolName");
			}
			var pool = _serverManager.ApplicationPools.FirstOrDefault(p => poolName.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
			if (pool != null) {
				ProcessUtils.RestartPool(pool, _serverManager);
			}
			else {
				throw  new ArgumentOutOfRangeException("poolName");
			}
			
		}

		public void FlushRedis(long siteId) {
			var site = GetSite(siteId);
			RedisUtils.FlushRedisDb(site.Redis.Host, site.Redis.Port, site.Redis.Db);
		}

		private const string AppHostConfigPath = @"\inetsrv\config\applicationhost.config";

		#endregion
	}
}