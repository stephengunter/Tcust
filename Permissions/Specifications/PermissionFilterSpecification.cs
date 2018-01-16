using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using ApplicationCore.Specifications;

namespace Permissions.Specifications
{
	

	public class PermissionFilterSpecification : BaseSpecification<Permission>
	{
		public PermissionFilterSpecification(string name)
		{
			Criteria = p => p.Name == name;
		}

		public PermissionFilterSpecification(IList<int> permissionIds)
		{
			Criteria = p => permissionIds.Contains(p.Id);
		}
	}

	
}
