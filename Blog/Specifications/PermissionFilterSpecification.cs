using ApplicationCore.Specifications;
using Blog.Models;
using ApplicationCore.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Blog.Specifications
{
	public class PermissionFilterSpecification : BaseSpecification<Permission>
	{
		public PermissionFilterSpecification(string name)
		{
			Criteria = p => p.Name == name;
		}
	}



	public class UserPermissionFilterSpecification : BaseSpecification<UserPermission>
	{
		public UserPermissionFilterSpecification(string userId, int permissionId)
		{
			Criteria = p => p.UserId == userId && p.PermissionId== permissionId;
		}
	}
		
}
