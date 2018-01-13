using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public  class Permisson : BaseEntity
	{
		public string Name { get; set; }
		public string Title { get; set; }

		
		public bool Removed { get; set; }
	}

	public class AppUser : BaseRecord
	{
		public string UserId { get; set; }
		public string Name { get; set; }
		public string PS { get; set; }

		public int PermissionId { get; set; }

		public ICollection<Permisson> Permissions { get; set; }

		public bool Activce { get; set; }
		public bool Removed { get; set; }
	}
}
