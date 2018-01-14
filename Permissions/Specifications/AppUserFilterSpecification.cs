using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using ApplicationCore.Specifications;
using ApplicationCore.Helpers;

namespace Permissions.Specifications
{
	public class BaseAppUserFilterSpecification : BaseSpecification<AppUser>
	{
		public BaseAppUserFilterSpecification()
		{
			Criteria = u => !u.Removed && u.Active;

			AddInclude(u => u.UserPermissions);


		}
	}
	public class AppUserFilterSpecification : BaseAppUserFilterSpecification
	{
		public AppUserFilterSpecification(string keyword)
		{
			var compiled = Criteria.Compile();
			Criteria = u => compiled(u) && u.Name.CaseInsensitiveContains(keyword);

		}
	}

	
}
