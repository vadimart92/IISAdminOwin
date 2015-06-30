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

		public string BuildFolderLink { get; set; }

		public string ZipFilePath {
			get {
				throw new NotImplementedException();
			}
		}
	}

	public class SignalrSqlServerInstance : ISqlServerInstance
	{
		public string ServerName { get; set; }

		public string InstanceName { get; set; }

		public string Version { get; set; }

		public string Name {
			get {
				return string.Format("{0}/{1}", ServerName, InstanceName);
			}
		}
	}
	public class SignalRRedis : IRedis
	{
		public string ConnectionString {get;set;}

		public int Db { get; set; }

		public string Host { get; set; }

		public int Port { get; set; }
	}
}