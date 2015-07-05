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

		public SignalRRedis RedisInfo {
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

		public string DestinationPath {
			get;
			set;
		}

		public string RedisConnectionString {
			get {
				return RedisInfo.ConnectionString;
			}
		}

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
}