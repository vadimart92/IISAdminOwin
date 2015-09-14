using System.Data.Entity;

namespace IISAdmin.Owin.DAL.EF
{
    public class JobQueueConext : DbContext
    {
        public JobQueueConext() : base("mainDb")
        {
            
        }

        public JobQueueConext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        public DbSet<JobInfo> JobInfos { get; set; }
    }
}
