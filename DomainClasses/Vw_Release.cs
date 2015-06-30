using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class Vw_Release
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BuildFolderLink { get; set; }
        public string Name { get; set; }
        public int IsPublished { get; set; }
        public Guid BuildId { get; set; }
    }
}
