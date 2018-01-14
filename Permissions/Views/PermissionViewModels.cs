using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;

namespace Permissions.Views
{
	public class UserViewModel
	{
		public string userId { get; set; }
		public string name { get; set; }
		public string ps { get; set; }

		public bool active { get; set; }
		public bool removed { get; set; }

		public int[] PermissionIds { get; set; }

		public List<PermissionViewModel> permissionViews { get; set; }

		public AppUser MapToEntity(string updatedBy, AppUser user = null)
		{
			if (user == null)
			{
				user = new AppUser();

			}


			user.UserId = userId;
			user.Name = name;
			user.PS = ps;
			user.Active = active;
			user.Removed = removed;

			user.SetUpdated(updatedBy);



			return user;
		}
	}

	public class PermissionViewModel
	{
		public string name { get; set; }
		public string title { get; set; }
		public bool removed { get; set; }


	}

	public class UserEditForm
	{
		 
		public UserViewModel user { get; set; }
	}

}
