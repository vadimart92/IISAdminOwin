using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DomainClasses
{
   public class ReleaseContext : DbContext, IDisposable
    {
        public ReleaseContext() : base("WorkDbContext")
        {}

        public DbSet<Vw_Release> Releases { get; set; }
        public DbSet<tbl_Build> Builds { get; set; } 

    }   
}
