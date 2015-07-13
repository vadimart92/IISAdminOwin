using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using WebSiteManagment.Core.Models;
using Site = Microsoft.Web.Administration.Site;

namespace WebSiteManagment.Core.Common
{
	public static class WebConfigUtils
	{
		public static string GetConnectionString(Site site, string name) {
			var siteRoot = GetSiteRoot(site);
			var config = OpenConfig(siteRoot);
			var cs = config != null ? config.ConnectionStrings.ConnectionStrings[name] : null;
			if (cs == null) {
				return string.Empty;
			}
			return cs.ConnectionString;
		}

		private static Configuration OpenConfig(string siteRoot) {
			var configFileName = Path.Combine(siteRoot, "web.config");
			if (!File.Exists(configFileName)) {
				return null;
			}
			var vdm = new VirtualDirectoryMapping(siteRoot, true);
			var wcfm = new WebConfigurationFileMap();
			wcfm.VirtualDirectories.Add("/", vdm);
			var config = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, @"/");
			if (config.FilePath != configFileName) {
				return null;
			}
			return config;
		}

		public static string GetSiteRoot(Site site) {
			var res = string.Empty;
			if (site != null) {
				var app = site.Applications.FirstOrDefault();
				if (app != null) {
					var dir = app.VirtualDirectories.FirstOrDefault();
					if (dir != null) {
						res = dir.PhysicalPath;
					}
				}
			}
			return res;
		}

		public static void SetSiteSettings(Site site, SiteSettings settings) {
			//todo setup site web.config & webapp/web.cofig
		}

		public static void SetConnectionString(string siteRoot, string csName, string newValue) {
			var config = OpenConfig(siteRoot);
			if (config == null) {
				return;
			}
			var cs = config.ConnectionStrings.ConnectionStrings[csName];
			if (cs == null) {
				return;
			}
			cs.ConnectionString = newValue;
			config.Save(ConfigurationSaveMode.Modified);
		}
		public static void SetConnectionString(Site site, string csName, string newValue) {
			var root = GetSiteRoot(site);
			if ( root != null && Directory.Exists(root)) {
				SetConnectionString(root, csName, newValue);
				if (csName == "db") {
					SetConnectionString(root, "reports", newValue);
				}
			}
		}
	}
}