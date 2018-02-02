using ApplicationCore.Helpers;
using ApplicationCore.Specifications;
using IdentityApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityApp.Specifications
{
	public class BaseUserFilterSpecifications : BaseSpecification<ApplicationUser>
	{
		public BaseUserFilterSpecifications()
		{
			AddInclude(u => u.Profile);
			Criteria = u => u.Id !=null;
		}
		
	}

	public class UserEmailFilterSpecifications : BaseUserFilterSpecifications
	{
		public UserEmailFilterSpecifications(string email)
		{
			Criteria = u => u.Email == email;
		}
	}

	public class UserIdFilterSpecifications : BaseUserFilterSpecifications
	{
		public UserIdFilterSpecifications(string id)
		{
			Criteria = u => u.Id == id;
		}
	}

	public class UserKeywordFilterSpecifications : BaseUserFilterSpecifications
	{
		public UserKeywordFilterSpecifications(string keyword)
		{
			Criteria = u => u.Profile.Fullname.CaseInsensitiveContains(keyword) ||
						   u.UserName.CaseInsensitiveContains(keyword);
		}
	}

	public class UserNameFilterSpecifications : BaseUserFilterSpecifications
	{
		public UserNameFilterSpecifications(string username)
		{
			Criteria = u => u.UserName == username;
		}
	}



}
