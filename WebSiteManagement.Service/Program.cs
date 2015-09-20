using System;
using System.ServiceProcess;
using System.Threading;
using log4net;

namespace WebSiteManagement.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            var log = LogManager.GetLogger("main");
            log.Debug("Crete inctance of WebSiteManagment");
            var service = new WebSiteManagment(log);
            log.Debug("Crete inctance of WebSiteManagment [Success]");
            var servicesToRun = new ServiceBase[]{ service };
#if DEBUG
            service.DebugRun();
#endif
#if !DEBUG
            log.Debug("Run service");
            try {
                ServiceBase.Run(servicesToRun);
                log.Debug("Run service [Success]");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
            catch (Exception ex) {
                log.Error("Error running service", ex);
                throw;
            }
            
            
#endif
        }
    }
}
