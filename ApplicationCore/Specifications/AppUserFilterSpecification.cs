using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Specifications
{
	public class BaseAppUserFilterSpecification : BaseSpecification<AppUser>
	{
		public BaseAppUserFilterSpecification()
		{
			Criteria = u => !u.Removed && u.Activce;

			AddInclude(u => u.Permissions);


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
