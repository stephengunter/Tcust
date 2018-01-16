using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Helpers;

namespace Permissions.Models
{
	public class AppUser : BaseRecord
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
		
		public string PS { get; set; }


		[NotMapped]
		public ICollection<Permission> Permissions { get; }

		private ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();

		public AppUser()
		{
			CreatedAt = DateTime.Now;

			Permissions = new JoinCollectionFacade<Permission, AppUser, UserPermission>(this, UserPermissions);
		}

		
	}
}
