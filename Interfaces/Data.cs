
namespace IISAdmin.Interfaces
{
	#region ISiteCreateData

	public interface ISiteCreateData {
		string Name { get; }

		string RedisConnectionString { get; }

		IRelease ReleaseInfo { get; }

		ISqlServerInstance Db { get; }

		string DestinationPath { get; }
		string DbBackupTempPath { get; }
	}

	#endregion ISiteCreateData
}
