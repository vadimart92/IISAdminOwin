using System;
using System.Dynamic;
using System.Runtime.Remoting.Contexts;
using IISAdmin.Interfaces;
using IISAdmin.Owin.Common;
using IISAdmin.Owin.Mock;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace IISAdmin.Owin.Test
{
    [TestClass]
    public class SiteCreateHub_Test
    {
        [TestMethod]
        public void TestMethod1() {
            bool messageInvoked = false;
            var container = (new MockContainerConfig((c) => {
                var contextMock = new Mock<IHubContext>();
                var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
                dynamic all = new ExpandoObject();
                all.updateSiteState = new Action<string, string>((name, message) => {
                    messageInvoked = true;
                });
                mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
                contextMock.Setup(context => context.Clients).Returns(mockClients.Object);
                var provider = new Mock<IHubContextProvider>();
                provider.Setup(p => p.GetConnectionContext("TestHub")).Returns(contextMock.Object);
                c.RegisterInstance(provider.Object);
                c.RegisterType<ISiteDeployProvider, SiteDeployProviderMock>();
            })).GetContainer();

            var _siteDeployProvider = container.Resolve<ISiteDeployProvider>();
            var data = new SiteCreateData {
                Name = "asdsadsa",
                Db = new SqlServerInstance {ServerName = "", InstanceName = ""}
            };
            data.UserName = "Test";
            OperationInfoBase progressInfo = _siteDeployProvider.GetOperationsInfo();
            progressInfo.SignarRHubName = "TestHub";
            progressInfo.OnStateChanged += (hub, info) => {
                var dataStr = JsonConvert.SerializeObject(info);
                hub.Clients.All.updateSiteState(dataStr);
            };
            var progressJobId = progressInfo.Id;
            _siteDeployProvider.DeployWebApp(data, progressJobId);
        }
    }
}
