using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using IISAdmin.Interfaces;
using Owin;

namespace IISAdmin.Hangfire
{
    public class HangfireWorker : IBackgroundWorker
    {
        public HangfireWorker(string connectionString) {
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
        }

        public void Init(IAppBuilder appBuilder) {
            appBuilder.UseHangfireServer();
            appBuilder.UseHangfireDashboard();
        }
        
    }
}
