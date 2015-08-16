using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IISAdmin.Interfaces
{

    public interface IJobInfoRepository : IRepository<OperationInfoBase, Guid>, IQueryableRepository<OperationInfoBase, Guid> {
        OperationInfoBase Get(string jobKey);
    }
}
