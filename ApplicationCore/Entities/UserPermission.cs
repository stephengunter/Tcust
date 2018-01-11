using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class  AppUser : BaseRecord
    {
		public string Name { get; set; }
		
		public bool Active { get; set; }

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

	

	public class Permission : BaseEntity
	{
		public string Name { get; set; }
		public string Title { get; set; }

		[NotMapped]
		public ICollection<AppUser> AppUsers { get; }

		private ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();
	}

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
