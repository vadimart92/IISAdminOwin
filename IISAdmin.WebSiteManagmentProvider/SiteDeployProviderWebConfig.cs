using System;
using System.Configuration;

namespace IISAdmin.WebSiteManagmentProvider {
	public class SiteDeployProviderWebConfig : ISiteDeployProviderWebConfig {

		public string WebAppRoot
		{
			get { return ConfigurationManager.AppSettings["WebAppRoot"]; }
		}

		public string DbBackupTempPath {
			get {
				return ConfigurationManager.AppSettings["DbBackupTempPath"];
			}
		}

		public bool NewSiteForBuild {
			get {
				return Boolean.Parse(ConfigurationManager.AppSettings["NewSiteForBuild"]);
			}
		}

		public string RedisTypicalConnectionString {
			get {
				return ConfigurationManager.AppSettings["RedisTypicalConnectionString"];
			}
		}
		public string DbTypicalConnectionString {
			get {
				return ConfigurationManager.AppSettings["DbTypicalConnectionString"];
			}
		}
	}
}
