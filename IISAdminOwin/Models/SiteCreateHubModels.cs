using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.Models {
	public class SignalrRelease: IRelease {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }
		public bool Release { get; set; }
		public DateTime CreatedOn { get; set; }
		public string BuildFolderLink { get; set; }
	}
	public class SignalrSqlServerInstance : ISqlServerInstance {
		public string ServerName { get; set; }
		public string InstanceName { get; set; }
		public string Version { get; set; }
		public string Name {
			get {
				return string.Format("{0}/{1}", ServerName, InstanceName);
			}
		}
	}
}