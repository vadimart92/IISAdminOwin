using IISAdmin.Interfaces;
using IISAdmin.Owin.Models;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs.SiteCreate {

	[JsonObject]
	public class SiteCreateData : ISiteCreateData {
		
		public string Name {
			get;
			set;
		}

		public string WebAppName { get; set; }

		public Redis RedisInfo {
			get;
			set;
		}

		public SignalrRelease ReleaseInfo {
			get;
			set;
		}

		public SignalrSqlServerInstance Db {
			get;
			set;
		}

		#region Члены ISiteCreateData

		public string DestinationWebAppRoot {
			get;
			set;
		}

		public bool SeparateFolder { get; set; }

		public string UserName { get; set; }
		
		public bool CreateNewSite { get; set; }

		IRelease ISiteCreateData.ReleaseInfo {
			get {
				return ReleaseInfo;
			}
		}

		ISqlServerInstance ISiteCreateData.Db {
			get {
				return Db;
			}
		}

		#endregion Члены ISiteCreateData
	}

	[JsonObject]
	public class ReleaseInfo {
		
		public IRelease Release { get; set; }
		public string WebAppName { get; set; }
		public string WebAppDir { get; set; }
	}

}