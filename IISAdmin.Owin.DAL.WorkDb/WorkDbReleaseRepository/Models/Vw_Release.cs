using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository.Models
{
	[Table("Vw_Release", Schema = "dbo")]
	public class WorkDbRelease : Release {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public override Guid Id {
			get;
			set;
		}

		[Column("Name")]
		public override string Name {
			get;
			set;
		}

		[Column("Version")]
		public override string Version {
			get;
			set;
		}

		[Column("Release")]
		public override bool IsRelease {
			get;
			set;
		}

		[Column("CreatedOn")]
		public override DateTime CreatedOn {
			get;
			set;
		}

		[Column("BuildFolderLink")]
		public override string ZipFilePath {
			get;
			set;
		}
	}
}