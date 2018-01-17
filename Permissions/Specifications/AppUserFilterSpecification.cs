using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using ApplicationCore.Specifications;
using ApplicationCore.Helpers;

namespace Permissions.Specifications
{
	public class AppUserFilterSpecification : BaseSpecification<AppUser>
	{
		public AppUserFilterSpecification(string keyword)
		{
			Criteria = u =>  ( u.Name.CaseInsensitiveContains(keyword) || u.Email.CaseInsensitiveContains(keyword) );


		}

		
	}
}
