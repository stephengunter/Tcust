using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using System.ComponentModel.DataAnnotations;

namespace Permissions.Views
{
	public class UserViewModel
	{
		public int id { get; set; }

		[Required(ErrorMessage = "請填寫Email")]
		public string email { get; set; }
		
		public string userId { get; set; }

		
		public string name { get; set; }

		public string ps { get; set; }
		public string updatedAt { get; set; }
		public string updatedBy { get; set; }

		public int[] permissionIds { get; set; }

		public List<PermissionViewModel> permissionViews { get; set; }

		public AppUser MapToEntity(string updatedBy, AppUser user = null)
		{
			if (user == null) user = new AppUser();

			user.Email = email;
			user.UserId = userId;
			user.Name = name;
			user.PS = ps;
		

			user.SetUpdated(updatedBy);



			return user;
		}
	}

	public class PermissionViewModel
	{
		public string name { get; set; }
		public string title { get; set; }
		public bool adminOnly { get; set; }


	}

	public class PermissionOption
	{
		public int value { get; set; }
		public string text { get; set; }
	}

	public class UserEditForm
	{
		 
		public UserViewModel user { get; set; }
		public List<PermissionOption> permissionOptions { get; set; }
	}

}
