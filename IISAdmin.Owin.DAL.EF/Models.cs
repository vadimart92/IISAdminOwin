using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.EF
{
    public class JobInfo {
        [Key]
        public Guid Id { get; set; }
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }
        public string SerializedInfo { get; set; }
        public string SignarRHubName { get; set; }
    }

    [Table("Job", Schema = "HangFire")]
    public class Job {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}