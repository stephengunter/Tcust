using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Helpers;

namespace Permissions.Models
{
	public class Permission : BaseEntity
	{
		public string Name { get; set; }
		public string Title { get; set; }

		public bool AdminOnly { get; set; }



		public Permission() => AppUsers = new JoinCollectionFacade<AppUser, Permission, UserPermission>(this, UserPermissions);


		[NotMapped]
		public ICollection<AppUser> AppUsers { get; }

		private ICollection<UserPermission> UserPermissions { get; } = new List<UserPermission>();

		
	}
}
