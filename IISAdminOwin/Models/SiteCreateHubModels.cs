using System;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.Models
{
	public class SignalrRelease : IRelease
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Version { get; set; }

		public bool Release { get; set; }

		public DateTime CreatedOn { get; set; }

		public string ZipFilePath { get; set; }

		public SignalrRelease(IRelease release) {
			Id = release.Id;
			Name = release.Name;
			CreatedOn = release.CreatedOn;
			Release = release.Release;
			Version = release.Version;
			ZipFilePath = release.ZipFilePath;
		}

		public SignalrRelease() {
			
		}
	}

	public class SignalrSqlServerInstance : ISqlServerInstance
	{
		public string ServerName { get; set; }

		public string InstanceName { get; set; }

		public string Version { get; set; }

		public string Name { get; set; }
	}

}