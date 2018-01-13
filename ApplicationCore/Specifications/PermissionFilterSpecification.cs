using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
	public class BasePermissionFilterSpecification : BaseSpecification<Permisson>
	{
		public BasePermissionFilterSpecification()
		{
			Criteria = u => !u.Removed;
		}
	}

	public class PermissionFilterSpecification: BasePermissionFilterSpecification
	{
		public PermissionFilterSpecification(string name)
		{
			Criteria = p => p.Name == name;
		}
	}
}
