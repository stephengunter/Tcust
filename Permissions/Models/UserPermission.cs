using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permissions.Models
{
	

	public class UserPermission : IJoinEntity<AppUser>, IJoinEntity<Permission>
	{
		public int AppUserId { get; set; }
		public AppUser AppUser { get; set; }
		AppUser IJoinEntity<AppUser>.Navigation
		{
			get => AppUser;
			set => AppUser = value;
		}

		public int PermissionId { get; set; }
		public Permission Permission { get; set; }
		Permission IJoinEntity<Permission>.Navigation
		{
			get => Permission;
			set => Permission = value;
		}


	}
}
