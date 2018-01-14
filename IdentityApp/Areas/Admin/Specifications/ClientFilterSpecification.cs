using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using ApplicationCore.Specifications;
using ApplicationCore.Helpers;

namespace IdentityApp.Areas.Admin.Specifications
{
	public class BaseClientFilterSpecification : BaseSpecification<Client>
	{	
		public BaseClientFilterSpecification()
		{
			Criteria = c => c.Id > 0;

			AddInclude(c => c.RedirectUris);
			AddInclude(c => c.PostLogoutRedirectUris);
		}
	}

	public class ClientFilterSpecification : BaseClientFilterSpecification
	{
		public ClientFilterSpecification(string keyword)
		{
			
			Criteria = c =>  c.ClientName.CaseInsensitiveContains(keyword);

		}

		public ClientFilterSpecification(int id)
		{
			Criteria = c => c.Id == id;
			AddInclude(c => c.RedirectUris);
			AddInclude(c => c.PostLogoutRedirectUris);
		}
	}

	



}
