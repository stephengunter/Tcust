using ApplicationCore.Specifications;
using IdentityApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityApp.Specifications
{
    public class UserFilterSpecifications : BaseSpecification<ApplicationUser>
	{
		public UserFilterSpecifications(string email)
		{
			AddInclude(u => u.Profile);
			Criteria = u => u.Email == email;
		}
	}

	 

	
}
