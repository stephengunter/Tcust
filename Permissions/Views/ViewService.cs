using Permissions.Models;
using System.Collections.Generic;

namespace Permissions.Views
{
    public class PermissionViewService
	{
		public static UserViewModel MapUserViewModel(AppUser user, List<Permission> permissions)
		{
			var model = new UserViewModel()
			{
				permissionViews =new List<PermissionViewModel>()
			};

			
			model.userId = user.UserId;
			model.updatedAt = user.LastUpdated.ToString();
			model.updatedBy = user.UpdatedBy;
		
			model.ps = user.PS;

			foreach (var item in permissions)
			{
				model.permissionViews.Add(MapPermissionViewModel(item));
			}

			return model;

		}
		public static PermissionViewModel MapPermissionViewModel(Permission permission)
		{
			var model = new PermissionViewModel()
			{
				 name= permission.Name,
				 title=permission.Title,
			     adminOnly= permission.AdminOnly
			};

			return model;
		}
		public static PermissionOption MapPermissionOption(Permission permission)
		{
			return new PermissionOption { value = permission.Id, text = permission.Title };
		}
	}
}
