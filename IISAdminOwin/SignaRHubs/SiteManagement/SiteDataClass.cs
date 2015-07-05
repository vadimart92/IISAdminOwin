using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IISAdmin.Owin.SignaRHubs.SiteManagement {
	partial class SiteManagementHub {

		private class SiteDataClass {
			public SiteDataClass() {
				Data = new Dictionary<string, object>();
			}

			public string Name {
				get;
				set;
			}

			public Dictionary<string, object> Data {
				get;
				set;
			}
		}
	}
	
}