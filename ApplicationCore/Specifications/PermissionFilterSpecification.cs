using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class PermissionFilterSpecification: BaseSpecification<Permission>
	{
		public PermissionFilterSpecification(string name)
		{
			Criteria = p => p.Name == name;
		}
	}
}
