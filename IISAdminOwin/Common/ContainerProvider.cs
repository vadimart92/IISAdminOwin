using System.Configuration;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository;
using IISAdmin.WCFWebSiteRepository;
using IISAdmin.WebSiteManagmentProvider;
using Microsoft.Practices.Unity;

namespace IISAdmin.Owin.Common
{
	public class ContainerProvider
	{
		public static UnityContainer GetContainer() {
			var container = new UnityContainer();
			ConfigurateContainer(container);
			return container;
		}

		private static void ConfigurateContainer(UnityContainer container) {
			var hierarchicalLifeManager = new HierarchicalLifetimeManager();
			container.RegisterType<WcfClientWebSiteRepository>(new InjectionConstructor("NetNamedPipeBinding_IWebSiteRepositoryService"));
			container.RegisterType<IWebSiteRepository, WcfClientWebSiteRepository>(hierarchicalLifeManager);
			var sqlConnectionConstructor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["WorkDbContext"]);
			container.RegisterType<ISqlConnectionProvider, WorkSqlConnectionProvider>("WorkSqlConnectionProvider", sqlConnectionConstructor);
			container.RegisterType<IReleaseRepository, WorkDbReleaseRepository>(hierarchicalLifeManager, SqlConnectionProviderFactory.GetFactory("WorkSqlConnectionProvider"));
			container.RegisterType<ISqlServerInstanceRepository, LocalSqlServerInstanceRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ISiteDeployProvider, SiteDeployProvider>();
        }
	}

	public static class SqlConnectionProviderFactory {
		public static InjectionFactory GetFactory(string sqlConnectionProviderAlias) {
			return new InjectionFactory(c => c.Resolve<ISqlConnectionProvider>(sqlConnectionProviderAlias));
		}
	}
}