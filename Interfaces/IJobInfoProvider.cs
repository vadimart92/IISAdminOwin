using System;

namespace IISAdmin.Interfaces
{

    public interface IJobInfoRepository : IRepository<OperationInfoBase, Guid>, IQueryableRepository<OperationInfoBase, Guid> {
        OperationInfoBase Get(string jobKey);
    }
}
