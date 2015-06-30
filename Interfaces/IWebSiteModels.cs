using System.Collections.Generic;

namespace IISAdmin.Interfaces
{
	public interface ISite : IEntity<long>
	{
		string Name { get; set; }

		string State { get; set; }

		IRedis Redis { get; set; }

		List<string> Bindings { get; set; }

		List<IApplication> Applications { get; set; }
	}

	public interface IApplication
	{
		string Name { get; set; }

		IAppPool Pool { get; set; }
	}

	public interface IAppPool
	{
		string Name { get; set; }

		string State { get; set; }
	}

	public interface IRedis
	{
		int Db { get; set; }

		string Host { get; set; }

		int Port { get; set; }

		string ConnectionString { get; set; }
	}
}