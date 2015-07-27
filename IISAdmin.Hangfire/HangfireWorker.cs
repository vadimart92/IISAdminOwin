using Hangfire;
using IISAdmin.Interfaces;
using Microsoft.Practices.Unity;
using Owin;

namespace IISAdmin.Hangfire
{
    public class HangfireWorker : IBackgroundWorker
    {
        public HangfireWorker(string connectionString, IUnityContainer container) {
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
            GlobalConfiguration.Configuration.UseActivator(new UnityJobActivator(container));
        }

        public void Init(IAppBuilder appBuilder) {
            appBuilder.UseHangfireServer();
            appBuilder.UseHangfireDashboard();
        }
        
    }
}
