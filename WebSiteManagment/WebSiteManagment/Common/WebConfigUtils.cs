using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Web.Administration;
using SysCon =System.Configuration;

namespace WebSiteManagment.Common {
	public static class WebConfigUtils {
		
		public static string GetRedisConnectionString (Site site) {
			var siteRoot = GetSiteRoot(site);
			var configFileName = Path.Combine(siteRoot, "web.config");
			if (!File.Exists(configFileName)) {
				return null;
			}
			var vdm = new VirtualDirectoryMapping(siteRoot, true);
			var wcfm = new WebConfigurationFileMap();
			wcfm.VirtualDirectories.Add("/", vdm);

			var config = System.Web.Configuration.WebConfigurationManager.OpenMappedWebConfiguration(wcfm, @"/");
			var cs = config.ConnectionStrings.ConnectionStrings["redis"];
			return cs.ConnectionString;
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
	}
}
