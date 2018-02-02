using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityApp.Specifications
{
    

	public class UserRoleFilterSpecifications : BaseSpecification<IdentityUserRole<string>>
	{
		public UserRoleFilterSpecifications(IdentityRole role)
		{
			Criteria = ur => ur.RoleId == role.Id;
		}

		public UserRoleFilterSpecifications(string userId)
		{
			Criteria = ur => ur.UserId == userId;
		}

	}
}
