using Microsoft.Web.Administration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebSiteManagment.Core.Common;
using WebSiteManagment.Core.Models;
using Site = WebSiteManagment.Core.Models.Site;

namespace WebSiteManagment.Core
{
    public class WebSiteManager
    {
        #region Members

        public ServerManager _serverManager;
        private static ConcurrentDictionary<long, Site> _siteCache = new ConcurrentDictionary<long, Site>();
        private static readonly object Synchronizer = new object();
        private bool _useCache = true;

        public WebSiteManager()
        {
            var sys32Dir = Environment.GetFolderPath(Environment.SpecialFolder.System);
            var filePath = sys32Dir + AppHostConfigPath;
            _serverManager = new ServerManager(filePath);
        }

        public void ClearCashe()
        {
            _siteCache.Clear();
        }

        public void SetCasheUsage(bool type)
        {
            lock (Synchronizer)
            {
                _useCache = type;
            }
        }

        public List<Site> GetWebsites()
        {
            lock (Synchronizer)
            {
                if (_useCache)
                {
                    if (_serverManager.Sites.Count == _siteCache.Count)
                    {
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
            var sites = _serverManager.Sites.ToList().ConvertAll(s => new Site(s, _serverManager.ApplicationPools.ToList()));
            AddSitesToCache(sites);
            return sites;
        }

        private static void AddSitesToCache(List<Site> sites)
        {
            Task.Run(() =>
            {
                foreach (var site in sites)
                {
                    _siteCache[site.Id] = site;
                }
            });
        }

        public Site GetSite(long id)
        {
            var site = FindSite(id);
            if (site != null)
            {
                return site;
            }
            throw new ArgumentOutOfRangeException("id");
        }

        public Site FindSite(long id)
        {
            Site res;
            if (_useCache && _siteCache.TryGetValue(id, out res))
            {
                return res;
            }
            var site = _serverManager.Sites.FirstOrDefault(s => s.Id == id);
            if (site != null)
            {
                res = new Site(site, _serverManager.ApplicationPools.ToList());
                if (_useCache)
                {
                    _siteCache[id] = res;
                }
                return res;
            }
            return null;
        }

        public List<AppPool> GetPools()
        {
            return _serverManager.ApplicationPools.ToList().ConvertAll(p => new AppPool(p));
        }

        public void StopSite(long id)
        {
            var site = _serverManager.Sites.FirstOrDefault(s => s.Id == id);
            if (site != null)
            {
                RemoveSiteFromCache(site);
                site.Stop();
            }
        }

        private void RemoveSiteFromCache(Microsoft.Web.Administration.Site site)
        {
            if (_useCache)
            {
                Site res;
                _siteCache.TryRemove(site.Id, out res);
            }
        }

        public void StartSite(long id)
        {
            var site = _serverManager.Sites.FirstOrDefault(s => s.Id == id);
            if (site != null)
            {
                RemoveSiteFromCache(site);
                site.Start();
            }
        }

        public void StopPool(string name)
        {
            var pool =
                _serverManager.ApplicationPools.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (pool != null)
            {
                _siteCache.Clear();
                ProcessUtils.StopPool(pool);
            }
        }

        public void StopPool(int index)
        {
            if (index < _serverManager.ApplicationPools.Count)
            {
                var pool = _serverManager.ApplicationPools[index];
                _siteCache.Clear();
                ProcessUtils.StopPool(pool);
            }
        }

        public void RestartPool(string poolName)
        {
            if (string.IsNullOrWhiteSpace(poolName))
            {
                throw new ArgumentOutOfRangeException("poolName");
            }
            var pool = _serverManager.ApplicationPools.FirstOrDefault(p => poolName.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
            if (pool != null)
            {
                ProcessUtils.RestartPool(pool, _serverManager);
            }
            else
            {
                throw new ArgumentOutOfRangeException("poolName");
            }
        }

        public void FlushRedis(long siteId)
        {
            var site = GetSite(siteId);
            RedisUtils.FlushRedisDb(site.Redis.Host, site.Redis.Port, site.Redis.Db);
        }

        public long AddSite(SiteInfo info)
        {
            var pool = _serverManager.ApplicationPools.Add(info.Name);
            var site = _serverManager.Sites.FirstOrDefault(s => string.Compare(s.Name, info.SiteName, StringComparison.OrdinalIgnoreCase) == 0);
            if (info.CreateNewSite || site == null)
            {
                var port = CommonUtils.SafeGetFreeTcpPort();
                site = _serverManager.Sites.Add(info.Name, info.WebAppDir, port);
                site.ApplicationDefaults.ApplicationPoolName = pool.Name;
            }
            else
            {
                return -1;
            }
            for (int i = 0; i < info.AppCount; i++)
            {
                var app = site.Applications.Add($@"/{i}", Path.Combine(info.WebAppDir, "Terrasoft.WebApp"));
                app.ApplicationPoolName = pool.Name;
            }
            _serverManager.CommitChanges();
            return site.Id;
        }

        public void RemoveSite(string siteName, bool stopPools = true, bool removePools = true)
        {
            var site = _serverManager.Sites[siteName];
            if (site != null)
            {
                var appList = site.Applications.ToList();
                var poolList = appList.ConvertAll(app => _serverManager.ApplicationPools[app.ApplicationPoolName]).Where(p => p != null).ToList();
                if (removePools || stopPools)
                {
                    poolList.ConvertAll(p => p.Name).ForEach(StopPool);
                }
                appList.ForEach(site.Applications.Remove);
                site.Stop();
                _serverManager.Sites.Remove(site);
                if (removePools)
                {
                    poolList.ForEach(_serverManager.ApplicationPools.Remove);
                }
                _serverManager.CommitChanges();
                Site res;
                _siteCache.TryRemove(site.Id, out res);
            }
        }

        public void ModifyConnectionStrings(long siteId, Dictionary<string, string> config)
        {
            var site = _serverManager.Sites.FirstOrDefault(s => s.Id == siteId);
            if (site != null)
            {
                foreach (var item in config)
                {
                    WebConfigUtils.SetConnectionString(site, item.Key, item.Value);
                }
            }
            Site res;
            _siteCache.TryRemove(siteId, out res);
        }

        private const string AppHostConfigPath = @"\inetsrv\config\applicationhost.config";

        #endregion Members
    }
}