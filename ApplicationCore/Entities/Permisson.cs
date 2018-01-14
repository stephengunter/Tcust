using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public  class Permission : BaseEntity
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

		

		public ICollection<UserPermission> UserPermissions { get; set; }

		public bool Active { get; set; }
		public bool Removed { get; set; }
	}

	public class UserPermission : BaseRecord
	{
		public int UserId { get; set; }
		public AppUser User { get; set; }

		public int PermissionId { get; set; }
		public Permission Permission { get; set; }

		public bool Removed { get; set; }
	}


	//ViewModels

	
}
