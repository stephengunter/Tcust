using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using ApplicationCore.Specifications;

namespace Permissions.Specifications
{
	public class BasePermissionFilterSpecification : BaseSpecification<Permission>
	{
		public BasePermissionFilterSpecification()
		{
			Criteria = u => !u.Removed;
		}
	}

	public class PermissionFilterSpecification : BasePermissionFilterSpecification
	{
		public PermissionFilterSpecification(string name)
		{
			Criteria = p => p.Name == name;
		}
	}

	
}
