using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;

namespace Permissions.Models
{
	public class AppUser : BaseRecord
	{
		public string UserId { get; set; }
		public string Name { get; set; }
		public string PS { get; set; }



		public ICollection<UserPermission> UserPermissions { get; set; }

		public bool Active { get; set; }
		public bool Removed { get; set; }
	}
}
