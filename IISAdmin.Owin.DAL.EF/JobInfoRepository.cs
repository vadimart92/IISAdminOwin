using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.EF
{
    public class JobInfoRepository: IJobInfoRepository {

        static JobInfoRepository() {
            Mapper.CreateMap<OperationInfoBase, JobInfo>();
        }

        private readonly JobQueueConext _conext;

        public JobInfoRepository(string connectionString) {
            _conext = new JobQueueConext(connectionString);
        }
        public void Create(OperationInfoBase entity) {
            var row = Mapper.Map<JobInfo>(entity);
            _conext.JobInfos.Add(row);
            _conext.SaveChanges();
        }

        public OperationInfoBase Get(Guid key) {
            var row = _conext.JobInfos.SingleOrDefault(r => r.Id == key);
            return Mapper.Map<OperationInfoBase>(row);
        }

        public IEnumerable<OperationInfoBase> GetTopThousand() {
            return _conext.JobInfos.Take(1000).ToList().ConvertAll(Mapper.Map<OperationInfoBase>);
        }

        public IEnumerable<OperationInfoBase> Get(Expression<Func<OperationInfoBase, bool>> expression) {
            return _conext.JobInfos.Select(r=> Mapper.Map<OperationInfoBase>(r)).Where(expression);
        }

        public void Update(Guid id, Action<OperationInfoBase> action) {
            var row = Get(id);
            if (row == null) {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            action(row);
            _conext.SaveChanges();
        }

        public void Delete(Guid key) {
            var row = _conext.JobInfos.SingleOrDefault(r => r.Id == key);
            _conext.JobInfos.Remove(row);
        }

        IQueryable IQueryableRepository<OperationInfoBase, Guid>.Get() {
            return _conext.JobInfos;
        }

        public OperationInfoBase Get(string jobKey) {
            var row = _conext.JobInfos.Select(Mapper.Map<OperationInfoBase>).SingleOrDefault(r => r.JobId == jobKey);
            return row;
        }
    }
}
