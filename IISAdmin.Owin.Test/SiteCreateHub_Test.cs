using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.Remoting.Contexts;
using IISAdmin.Interfaces;
using IISAdmin.Owin.Common;
using IISAdmin.Owin.Mock;
using IISAdmin.WebSiteManagmentProvider;
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
            var infoRepo = new JobInfoRepositoryMock();
            var container = (new MockContainerConfig((c) => {
                var contextMock = new Mock<IHubContext>();
                var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
                dynamic all = new ExpandoObject();
                mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
                contextMock.Setup(context => context.Clients).Returns(mockClients.Object);
                var provider = new Mock<IHubContextProvider>();
                provider.Setup(p => p.GetConnectionContext("TestHub")).Returns(contextMock.Object);
                int counter = 0;
                all.updateSiteState = new Action<Guid>((infoId) => {
                    var info = new DeploySiteInfo(infoRepo, provider.Object, infoId);
                    lock (this)
                    {
                        switch (counter)
                        {
                            case 0:
                                Assert.AreEqual(info.RestoreDbCopyFiles, OperationStageState.Completed);
                                Assert.AreEqual(info.CreateWebApp, OperationStageState.Pending);
                                Assert.AreEqual(info.ModifyConfigs, OperationStageState.Pending);
                                counter++;
                                break;
                            case 1:
                                Assert.AreEqual(info.RestoreDbCopyFiles, OperationStageState.Completed);
                                Assert.AreEqual(info.CreateWebApp, OperationStageState.Completed);
                                Assert.AreEqual(info.ModifyConfigs, OperationStageState.Pending);
                                counter++;
                                break;
                            case 2:
                                Assert.AreEqual(info.RestoreDbCopyFiles, OperationStageState.Completed);
                                Assert.AreEqual(info.CreateWebApp, OperationStageState.Completed);
                                Assert.AreEqual(info.ModifyConfigs, OperationStageState.Completed);
                                break;
                        }
                    }
                });
                c.RegisterInstance(provider.Object);
                c.RegisterType<ISiteDeployProvider, SiteDeployProviderMock>();
                c.RegisterInstance<IJobInfoRepository>(infoRepo);
            })).GetContainer();
            var siteDeployProvider = container.Resolve<ISiteDeployProvider>();
            var data = new SiteCreateData {
                Name = "asdsadsa",
                Db = new SqlServerInstance {ServerName = "", InstanceName = ""},
                UserName = "Test"
            };
            OperationInfoBase progressInfo = siteDeployProvider.GetOperationsInfo();
            progressInfo.SignarRHubName = "TestHub";
            progressInfo.OnStateChanged += (hub, info) => {
                hub.Clients.All.updateSiteState(info.Id);
            };
            var progressJobId = progressInfo.Id;
            infoRepo.Create(progressInfo);
            siteDeployProvider.DeployWebApp(data, progressJobId);
        }
    }
}
