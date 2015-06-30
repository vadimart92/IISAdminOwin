using System;

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