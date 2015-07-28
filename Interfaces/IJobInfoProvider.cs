using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IISAdmin.Interfaces
{
    public interface IJobInfo:IEntity<Guid> {
        int JobId { get; set; }
        string SerializedInfo { get; set; }
    }

    public interface IJobInfoRepository : IRepository<IJobInfo, Guid>, IQueryableRepository<IJobInfo, Guid> {
        IJobInfo Get(int jobKey);
    }
}
