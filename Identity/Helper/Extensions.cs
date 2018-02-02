using System;
using System.Collections.Generic;
using System.Text;
using IdentityApp.Views;
using IdentityApp.Models;
using ApplicationCore.Paging;
using ApplicationCore.Views;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace IdentityApp.Helper
{
    public static class Extensions
    {
		public static IEnumerable<ApplicationUser> GetOrdered(this IEnumerable<ApplicationUser> users)
		{
			return ViewService.GetOrdered(users);
		}

		public static PagedList<ApplicationUser, IdentityUserViewModel> GetUsersPagedList(this IEnumerable<ApplicationUser> users, int page, int pageSize)
		{
			return ViewService.GetUsersPagedList(users, page, pageSize);
		}

		public static IdentityUserViewModel MapIdentityUserViewModel(this ApplicationUser user)
		{
			return ViewService.MapIdentityUserViewModel(user);
		}

		public static List<BaseOption> ToOptions(this IEnumerable<IdentityRole> roles,bool withEmpty = true)
		{
			var options = roles.Select(r => new BaseOption(r.Name, r.Name)).ToList();
			if (withEmpty) options.Insert(0, new BaseOption("", "所有角色"));
			return options;
		}

		//public static bool HasDevRole(this ApplicationUser user)
		//{

		//}

	}
}
