using Permissions.Models;

namespace Permissions.Views
{
    public class PermissionViewService
	{
		public static UserViewModel MapUserViewModel(AppUser user)
		{
			var model = new UserViewModel();
			model.userId = user.UserId;
			model.removed = user.Removed;
			model.active = user.Active;
			model.name = user.Name;
			model.ps = user.PS;

			return model;

		}
	}
}
