using System;
using System.Data.Entity;

namespace DomainClasses
{
	public class ReleaseContext : DbContext, IDisposable
	{
		public ReleaseContext() : base("WorkDbContext") {
		}

		public DbSet<Vw_Release> Releases { get; set; }

		public DbSet<tbl_Build> Builds { get; set; }
	}
}