using System.Collections.Generic;
using IISAdmin.Interfaces;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs.SiteCreate {

	[JsonObject]
	public class SiteCreationInfo {

		public IList<ISqlServerInstance> SqlServerInstances {
			get;
			set;
		}
	}
}