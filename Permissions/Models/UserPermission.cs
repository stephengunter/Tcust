using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permissions.Models
{
	public class UserPermission : BaseRecord
	{
		public int UserId { get; set; }
		public AppUser User { get; set; }

		public int PermissionId { get; set; }
		public Permission Permission { get; set; }

		public bool Removed { get; set; }
	}
}
