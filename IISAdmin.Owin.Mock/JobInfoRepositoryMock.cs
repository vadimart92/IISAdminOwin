using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.Mock
{
    public class JobInfoRepositoryMock:IJobInfoRepository
    {
        private Dictionary<Guid, OperationInfoBase> _store = new Dictionary<Guid, OperationInfoBase>();  
        public void Create(OperationInfoBase entity) {
            _store[entity.Id] = entity;
        }

        public OperationInfoBase Get(Guid key) {
            return _store[key];
        }

        public IEnumerable<OperationInfoBase> GetTopThousand() {
            throw new NotImplementedException();
        }

        public IEnumerable<OperationInfoBase> Get(Expression<Func<OperationInfoBase, bool>> expression) {
            throw new NotImplementedException();
        }

        public void Update(Guid key, Action<OperationInfoBase> action) {
            action(_store[key]);
        }

        public void Delete(Guid key) {
            throw new NotImplementedException();
        }

        public IQueryable Get() {
            throw new NotImplementedException();
        }

        public OperationInfoBase Get(string jobKey) {
            throw new NotImplementedException();
        }
    }
}
