using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using ApplicationCore.Specifications;
using ApplicationCore.Helpers;


namespace Permissions.Specifications
{
	
	public class UserPermissionFilterSpecification : BaseSpecification<UserPermission>
	{
		public UserPermissionFilterSpecification(int userId, int permissionId)
		{
			Criteria = up => up.AppUserId == userId && up.PermissionId == permissionId;
		}

		public UserPermissionFilterSpecification(AppUser user)
		{
			
			Criteria = up => up.AppUserId == user.Id;
		}

		public UserPermissionFilterSpecification(Permission permission)
		{
			
			Criteria = up => up.PermissionId == permission.Id;
		}

		public UserPermissionFilterSpecification(AppUser user, IList<int> permissionIds)
		{
			
			Criteria = up => up.AppUserId == user.Id && permissionIds.Contains(up.PermissionId);
		}
	}
}
