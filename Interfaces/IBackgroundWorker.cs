using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Owin;

namespace IISAdmin.Interfaces
{
    public interface IBackgroundWorker {
        void Init(IAppBuilder appBuilder);
        void AddJob<T>(Expression<Action<T>> expreissonJob, OperationInfoBase operationInfo);
    }
}
