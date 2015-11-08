using System;
using System.Linq.Expressions;
using Owin;

namespace IISAdmin.Interfaces
{
    public interface IBackgroundWorker {
        void Init(IAppBuilder appBuilder);
        void AddJob<T>(Expression<Action<T>> expreissonJob, OperationInfoBase operationInfo);
    }
}
