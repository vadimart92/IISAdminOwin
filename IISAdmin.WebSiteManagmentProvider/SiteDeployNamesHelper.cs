using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IISAdmin.Interfaces;

namespace IISAdmin.WebSiteManagmentProvider {
	public static class SiteDeployNamesHelper {

		public class DeployNamesInfo {
			public string ProductName { get; set; }
			public string Type { get; set; }
			public string ShortVersion { get; set; }
			public Version Version { get; set; }
			public string Locale { get; set; }
			public string DbType { get; set; }
			public string UserInitials { get; set; }
			public string WebAppName
			{
				get
				{
					return string.Format("{1}{0}{2}{0}{3}{0}{4}{5}{0}{6}{0}{7}", "_", ProductName, Type, Locale, DbType, ShortVersion, Version.Revision, UserInitials);
				}
			}

		}

		public static DeployNamesInfo GetDeployNamesInfo(ISiteCreateData inputData, string webAppRoot) {
			var res = new DeployNamesInfo();
			Version version;
			var vstr = GetShortVersion(inputData, out version);
			res.Version = version;
			res.ShortVersion = vstr;
			var nameTags = inputData.ReleaseInfo.Name.Split('_');
			if (nameTags.Length == 6) {
				res.ProductName = GetShortName(nameTags[1]);
				res.Type = nameTags[2];
				res.Locale = nameTags[3];
				var db = nameTags[4];
				res.DbType = (db == "MSSQL" ? String.Empty : db + "_");
				res.UserInitials = GetUserInitials(inputData.UserName);
			}
			return res;
		}

		public static string GetShortVersion(ISiteCreateData inputData, out  Version version) {
			var vstr = inputData.ReleaseInfo.Version ?? "0.0.0.0";
			Version.TryParse(vstr, out version);
			if (version == null) {
				version = new Version("0.0.0.0");
			}
			return String.Format("{0}{1}{2}", version.Major, version.Minor, version.Build);
		}

		public static string GetShortName(string name) {
			name = name.Replace(" ", string.Empty);
			if (name.Length > 16) {
				name = Regex.Matches(name, @"(\p{Lu}[a-z][a-z])|&").Cast<Match>().ToList().ConvertAll(m => m.Value).Aggregate((a, b) => a + b).Replace("&", "_");
			}
			return name;
		}

		public static string GetUserInitials(string name) {
			var indexOfPoint = name.IndexOf(".", StringComparison.OrdinalIgnoreCase);
			if (indexOfPoint > -1) {
				name = name.Substring(indexOfPoint - 1, 1) + name.Substring(indexOfPoint + 1, 1);
			} else if (name.Length > 4) {
				name = name.Substring(0, 4);
			}
			return name.ToLowerInvariant();
		}

	}
}
