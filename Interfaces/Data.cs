
namespace IISAdmin.Interfaces
{
	#region ISiteCreateData

	public interface ISiteCreateData {

		string Name { get; }

		string WebAppName { get; set; }

		IRelease ReleaseInfo { get; }

		ISqlServerInstance Db { get; }

		Redis RedisInfo { get; set; }

		string DestinationWebAppRoot { get; set; }
		
		bool SeparateFolder  { get; }

		string UserName { get; set; }

		bool CreateNewSite { get; set; }
	}

	#endregion ISiteCreateData
}
