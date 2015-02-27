using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL.Common;
using IISAdmin.WCFWebSiteRepository;
using Microsoft.Practices.Unity;

namespace IISAdmin.Owin.Common {
	public class ContainerProvider {
		
		public static UnityContainer GetContainer() {
			var container = new UnityContainer();
			ConfigurateContainer(container);
			return container;
		}

		private static void ConfigurateContainer(UnityContainer container) {
			container.RegisterType<WcfClientWebSiteRepository>(new InjectionConstructor("NetNamedPipeBinding_IWebSiteRepositoryService"));
			container.RegisterType<IWebSiteRepository, WcfClientWebSiteRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<IReleaseRepository, WorkDbReleaseRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<ISqlServerInstanceRepository, LocalSqlServerInstanceRepository>(new HierarchicalLifetimeManager());
		}
	}
}