using IISAdmin.Interfaces;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs.SiteCreate {
    
	[JsonObject]
	public class ReleaseInfo {
		
		public Release Release { get; set; }
		public string WebAppName { get; set; }
		public string WebAppDir { get; set; }
	}

}