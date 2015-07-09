using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.WorkDbReleaseRepository.Models
{
	[Table("Vw_Release", Schema = "dbo")]
	public class WorkDbRelease : IRelease {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id {
			get;
			set;
		}

		[Column("Name")]
		public string Name {
			get;
			set;
		}

		[Column("Version")]
		public string Version {
			get;
			set;
		}

		[Column("Release")]
		public bool Release {
			get;
			set;
		}

		[Column("CreatedOn")]
		public DateTime CreatedOn {
			get;
			set;
		}

		[Column("BuildFolderLink")]
		public string ZipFilePath {
			get;
			set;
		}
	}
}