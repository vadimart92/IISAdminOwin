using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using IISAdmin.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using IISAdmin.Owin.Common;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository.Models;
using IISAdmin.Owin.SignaRHubs.SiteManagement;
using IISAdminOwin;
using IISAdminOwin.Common;
using IISAdminOwin.DI;

namespace IISAdmin.Owin
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder) {
			var container = ContainerConfig.GetContainer();

			appBuilder.UseErrorPage();
			appBuilder.UseCors(CorsOptions.AllowAll);

			appBuilder.MapSignalR(initSignalRConfiguration(container));

		    var backgroundWorker = container.Resolve<IBackgroundWorker>();
            backgroundWorker.Init(appBuilder);

            var config = GetWebApiConfig(container);
			appBuilder.UseWebApi(config);

			BundlesRegistrator.RegisterBundles(appBuilder);

			appBuilder.Run(context => {
				var indexFileName = @"index.html";
				var homePath = @"/";
				if (context.Request.Path.Value == homePath) {
					return context.Response.WriteAsync(File.ReadAllText(Path.Combine(BundlesRegistrator.GetResourceRootDir(), indexFileName)));
				}
				context.Response.ContentType = "text/html";
				context.Response.Redirect(homePath);
				return context.Response.WriteAsync(string.Empty);
			});
		}

		private HttpConfiguration GetWebApiConfig(UnityContainer container) {
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "BreezeApi",
				routeTemplate: "breeze/{controller}/{action}"
			);
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new {
					id = RouteParameter.Optional
				}
			);

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

			config.DependencyResolver = new UnityResolver(container);

			return config;
		}

		private HubConfiguration initSignalRConfiguration(UnityContainer container) {
			var serializerSettings = new JsonSerializerSettings {
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				PreserveReferencesHandling = PreserveReferencesHandling.Objects,

				ContractResolver = new FilteredCamelCasePropertyNamesContractResolver {
					AssembliesToInclude = { typeof(SiteOperationState).Assembly, typeof(WorkDbRelease).Assembly }
				}
			};
			var serializer = JsonSerializer.Create(serializerSettings);

			GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
			GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(container));

			var config = new HubConfiguration { EnableJSONP = true, EnableDetailedErrors = true, EnableJavaScriptProxies = true };

			return config;
		}
	}
}