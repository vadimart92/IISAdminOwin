
namespace IISAdmin.WebSiteManagmentProvider {
	public interface ISiteDeployProviderWebConfig {
		string WebAppRoot { get; }
		string DbBackupTempPath { get; }
		bool NewSiteForBuild { get; }
		string RedisTypicalConnectionString { get; }
		string DbTypicalConnectionString { get; }
	}
}
