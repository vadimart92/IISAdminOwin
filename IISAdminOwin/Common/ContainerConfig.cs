﻿using System.Configuration;
using IISAdmin.BackgroundWorker;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL;
using IISAdmin.Owin.DAL.Dapper;
using IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository;
using IISAdmin.Owin.DAL.EF;
using IISAdmin.Owin.Mock;
using IISAdmin.WCFWebSiteRepository;
using IISAdmin.WebSiteManagmentProvider;
using Microsoft.Practices.Unity;

namespace IISAdmin.Owin.Common
{
	public class ContainerConfig
	{
		public static UnityContainer GetContainer() {
			var container = new UnityContainer();
			ConfigurateContainer(container);
			return container;
		}

		private static void ConfigurateContainer(UnityContainer container) {
			container.RegisterInstance(typeof(IUnityContainer), container);
            container.RegisterType<WcfClientWebSiteRepository>(new InjectionConstructor("NetNamedPipeBinding_IWebSiteRepositoryService"));
			container.RegisterType<IWebSiteRepository, WcfClientWebSiteRepository>(new HierarchicalLifetimeManager());
			var sqlConnectionConstructor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["WorkDbContext"].ConnectionString);
			container.RegisterType<ISqlConnectionProvider, WorkSqlConnectionProvider>("WorkSqlConnectionProvider", sqlConnectionConstructor);
			container.RegisterType<IReleaseRepository, WorkDbReleaseRepository>(new HierarchicalLifetimeManager(), new InjectionConstructor(container.Resolve<ISqlConnectionProvider>("WorkSqlConnectionProvider")));
			container.RegisterType<ISqlServerInstanceRepository, LocalSqlServerInstanceRepository>(new PerThreadLifetimeManager());
			container.RegisterType<ISiteDeployProviderWebConfig, SiteDeployProviderWebConfig>();
            container.RegisterType<ISiteDeployProvider, SiteDeployProviderMock>();
            container.RegisterType<IBackgroundWorker, HangfireWorker>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString, new ResolvedParameter<IJobInfoRepository>(), new ResolvedParameter<IUnityContainer>()));
            container.RegisterType<IJobInfoRepository, JobInfoRepository>(new InjectionConstructor(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString));
        }
	}
}