using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Hangfire;
using IISAdmin.Interfaces;
using Microsoft.Practices.Unity;
using Owin;

namespace IISAdmin.BackgroundWorker
{
    public class HangfireWorker : IBackgroundWorker {
        private IJobInfoRepository _jobInfoRepository;
        public HangfireWorker(string connectionString, IJobInfoRepository infoRepository, IUnityContainer container) {
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
            Contract.Requires(infoRepository != null, "infoRepository != null");
            _jobInfoRepository = infoRepository;
            GlobalConfiguration.Configuration.UseActivator(new UnityJobActivator(container));
        }

        public void Init(IAppBuilder appBuilder) {
            appBuilder.UseHangfireServer();
            appBuilder.UseHangfireDashboard();
        }

        public void AddJob<T>(Expression<Action<T>> expreissonJob, OperationInfoBase operationInfo) {
            var jobId = BackgroundJob.Schedule(expreissonJob, TimeSpan.FromSeconds(5));
            operationInfo.JobId = jobId;
            operationInfo.Save();
        }
    }
}
