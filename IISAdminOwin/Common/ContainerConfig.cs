using System;
using System.Configuration;
using IISAdmin.BackgroundWorker;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL;
using IISAdmin.Owin.DAL.Dapper;
using IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository;
using IISAdmin.Owin.DAL.EF;
using IISAdmin.Owin.Mock;
using IISAdmin.Owin.SignaRHubs;
using IISAdmin.WCFWebSiteRepository;
using IISAdmin.WebSiteManagmentProvider;
using Microsoft.Practices.Unity;

namespace IISAdmin.Owin.Common
{
	public class ContainerConfig {
	    protected UnityContainer _container;
		public UnityContainer GetContainer() {
            _container = new UnityContainer();
			ConfigurateContainer();
			return _container;
		}

		protected virtual void ConfigurateContainer() {
            _container.RegisterType<WcfClientWebSiteRepository>(new InjectionConstructor("NetNamedPipeBinding_IWebSiteRepositoryService"));
			_container.RegisterType<IWebSiteRepository, WcfClientWebSiteRepository>(new HierarchicalLifetimeManager());
			var sqlConnectionConstructor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["WorkDbContext"].ConnectionString);
			_container.RegisterType<ISqlConnectionProvider, WorkSqlConnectionProvider>("WorkSqlConnectionProvider", sqlConnectionConstructor);
			_container.RegisterType<IReleaseRepository, WorkDbReleaseRepository>(new HierarchicalLifetimeManager(), new InjectionConstructor(_container.Resolve<ISqlConnectionProvider>("WorkSqlConnectionProvider")));
			_container.RegisterType<ISqlServerInstanceRepository, LocalSqlServerInstanceRepository>(new PerThreadLifetimeManager());
			_container.RegisterType<ISiteDeployProviderWebConfig, SiteDeployProviderWebConfig>();
            _container.RegisterType<ISiteDeployProvider, SiteDeployProviderMock>();
            _container.RegisterType<IBackgroundWorker, HangfireWorker>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString, new ResolvedParameter<IJobInfoRepository>(), new ResolvedParameter<IUnityContainer>()));
            _container.RegisterType<IJobInfoRepository, JobInfoRepository>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString));
            _container.RegisterType<IHubContextProvider, SingnalRHubContextProvider>();
        }
	}

    public class MockContainerConfig: ContainerConfig {

        private Action<UnityContainer> _action;
        public MockContainerConfig(Action<UnityContainer> action = null) {
            _action = action;
        }

        protected override void ConfigurateContainer() {
            _action?.Invoke(_container);
            var cs = "Data Source=ArtemchukPC;Initial Catalog=IISAdmin;Integrated Security=True";
            _container.RegisterType<WcfClientWebSiteRepository>(new InjectionConstructor("NetNamedPipeBinding_IWebSiteRepositoryService"));
            RegisterIfNot<IWebSiteRepository>(()=> _container.RegisterType<IWebSiteRepository, WcfClientWebSiteRepository>(new HierarchicalLifetimeManager()));
            RegisterIfNot<ISqlConnectionProvider>(() => _container.RegisterType<ISqlConnectionProvider, WorkSqlConnectionProvider>("WorkSqlConnectionProvider", new InjectionConstructor(cs)));
            RegisterIfNot<IReleaseRepository>(() => _container.RegisterType<IReleaseRepository, WorkDbReleaseRepository>(new HierarchicalLifetimeManager(), new InjectionConstructor(_container.Resolve<ISqlConnectionProvider>("WorkSqlConnectionProvider"))));
            RegisterIfNot<ISqlServerInstanceRepository>(() => _container.RegisterType<ISqlServerInstanceRepository, LocalSqlServerInstanceRepository>(new PerThreadLifetimeManager()));
            RegisterIfNot<ISiteDeployProviderWebConfig>(() => _container.RegisterType<ISiteDeployProviderWebConfig, SiteDeployProviderWebConfig>());
            RegisterIfNot<ISiteDeployProvider>(() => _container.RegisterType<ISiteDeployProvider, SiteDeployProviderMock>());
            RegisterIfNot<IBackgroundWorker>(() => _container.RegisterType<IBackgroundWorker, HangfireWorker>(new InjectionConstructor(cs, new ResolvedParameter<IJobInfoRepository>(), new ResolvedParameter<IUnityContainer>())));
            RegisterIfNot<IJobInfoRepository>(() => _container.RegisterType<IJobInfoRepository, JobInfoRepository>(new InjectionConstructor(cs)));
            RegisterIfNot<IHubContextProvider>(() => _container.RegisterType<IHubContextProvider, SingnalRHubContextProvider>());
        }

        private void RegisterIfNot<T>(Action registerAction) {
            if (!_container.IsRegistered(typeof (T))) {
                registerAction();
            }
        }
    }
}