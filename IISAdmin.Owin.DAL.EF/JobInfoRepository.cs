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
            Mapper.CreateMap<IJobInfo, JobInfo>();
        }

        private readonly JobQueueConext _conext;

        public JobInfoRepository(string connectionString) {
            _conext = new JobQueueConext(connectionString);
        }
        public void Create(IJobInfo entity) {
            var row = Mapper.Map<JobInfo>(entity);
            _conext.JobInfos.Add(row);
            _conext.SaveChanges();
        }

        public IJobInfo Get(Guid key) {
            var row = _conext.JobInfos.SingleOrDefault(r => r.Id == key);
            return row;
        }

        public IEnumerable<IJobInfo> GetTopThousand() {
            return _conext.JobInfos.Take(1000);
        }

        public IEnumerable<IJobInfo> Get(Expression<Func<IJobInfo, bool>> expression) {
            return _conext.JobInfos.Where(expression);
        }

        public void Update(Guid id, Action<IJobInfo> action) {
            var row = Get(id);
            action(row);
            _conext.SaveChanges();
        }

        public void Delete(Guid key) {
            var row = _conext.JobInfos.SingleOrDefault(r => r.Id == key);
            _conext.JobInfos.Remove(row);
        }

        IQueryable IQueryableRepository<IJobInfo, Guid>.Get() {
            return _conext.JobInfos;
        }

        public IJobInfo Get(int jobKey) {
            var row = _conext.JobInfos.SingleOrDefault(r => r.JobId == jobKey);
            return row;
        }
    }
}
