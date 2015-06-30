using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IISAdmin.Owin.SignaRHubs
{
	public enum OperationState
	{
		Running, Restarting, Stopped
	}

	[JsonObject]
	public class SiteOperationState
	{
		private DateTime? _operationTime;

		[JsonProperty]
		public long Id { get; set; }

		[JsonProperty]
		public DateTime OperationTime
		{
			get { return _operationTime ?? DateTime.UtcNow; }
			set { _operationTime = value; }
		}

		[JsonProperty]
		public Dictionary<string, object> NewData = new Dictionary<string, object>();
	}
}