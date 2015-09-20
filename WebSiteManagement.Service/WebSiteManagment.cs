using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using WebSiteManagment.Wcf;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace WebSiteManagement.Service
{
    public partial class WebSiteManagment : ServiceBase {
        private ServiceHost _serviceHost;
        private ILog _log;
        public WebSiteManagment(ILog log) {
            _log = log;
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            _log.Info("Creating service host");
            try {
                _serviceHost = new ServiceHost(typeof(WebSiteManagementService));
            }
            catch (Exception ex) {
                _log.Error("Failed to create service host", ex);
                throw;
            }
            
            _log.Info("Starting WebSiteManagementService");
            _serviceHost.Opened += _serviceHost_Opened;
            _serviceHost.Faulted += _serviceHost_Faulted;
            _serviceHost.Open(TimeSpan.FromSeconds(30));
        }

        private void _serviceHost_Faulted(object sender, EventArgs e)
        {
            _log.Error("WebSiteManagement Service Host Faulted");
        }

        private void _serviceHost_Opened(object sender, EventArgs e)
        {
            _log.Info("WebSiteManagement Service Host opened");
        }

        protected override void OnStop()
        {
            _log.Info("Stopping WebSiteManagementService");
            if (_serviceHost != null) {
                _serviceHost.Close(TimeSpan.FromSeconds(30));
                _serviceHost = null;
                _log.Info("Service host stoped");
            }
        }
#if DEBUG
        public void DebugRun() {
            var task = Task.Factory.StartNew(() => {
                OnStart(new string[0]);
                _log.Debug("WCF started");
                Thread.Sleep(TimeSpan.FromMinutes(2));
            });
            task.Wait();
            OnStop();
            _log.Debug("WCF stoped");
        }
#endif
    }
}
