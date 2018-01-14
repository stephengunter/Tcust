using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using ApplicationCore.Specifications;
using ApplicationCore.Helpers;


namespace Permissions.Specifications
{
	public class BaseUserPermissionFilterSpecification : BaseSpecification<UserPermission>
	{
		public BaseUserPermissionFilterSpecification()
		{
			Criteria = u => !u.Removed;
		}
	}
	public class UserPermissionFilterSpecification : BaseUserPermissionFilterSpecification
	{
		public UserPermissionFilterSpecification(int userId, int permissionId)
		{
			var compiled = Criteria.Compile();
			Criteria = up => up.UserId == userId && up.PermissionId == permissionId;
		}

		public UserPermissionFilterSpecification(AppUser user)
		{
			var compiled = Criteria.Compile();
			Criteria = up => up.UserId == user.Id;
		}

		public UserPermissionFilterSpecification(Permission permission)
		{
			var compiled = Criteria.Compile();
			Criteria = up => up.PermissionId == permission.Id;
		}

		public UserPermissionFilterSpecification(AppUser user, IList<int> permissionIds)
		{
			var compiled = Criteria.Compile();
			Criteria = up => up.UserId == user.Id && permissionIds.Contains(up.PermissionId);
		}
	}
}
