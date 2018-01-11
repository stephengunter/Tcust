using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
    public class AppUserFilterSpecification : BaseSpecification<AppUser>
	{
		public AppUserFilterSpecification(string name)
		{
			Criteria = u => u.Name == name;
		}
	}
}
