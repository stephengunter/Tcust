using System;
using System.Collections.Generic;
using System.Text;
using IdentityApp.Views;
using IdentityApp.Models;
using System.Linq;
using ApplicationCore.Paging;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.Helpers;

namespace IdentityApp.Helper
{
    public class ViewService
    {
		public static IEnumerable<ApplicationUser> GetOrdered(IEnumerable<ApplicationUser> users)
		{
			return users.OrderByDescending(u => u.CreatedAt);
		}
		public static PagedList<ApplicationUser, IdentityUserViewModel> GetUsersPagedList(IEnumerable<ApplicationUser> users, int page, int pageSize)
		{
			var pageList = new PagedList<ApplicationUser, IdentityUserViewModel>(users, page, pageSize);

			pageList.ViewList = new List<IdentityUserViewModel>();
			foreach (var item in pageList.List)
			{
				pageList.ViewList.Add(MapIdentityUserViewModel(item));
			}

			pageList.List = null;

			return pageList;
		}

		public static IdentityUserViewModel MapIdentityUserViewModel(ApplicationUser user, IEnumerable<IdentityRole> roles=null)
		{
			
			var model = new IdentityUserViewModel
			{
				id = user.Id,
				name= user.UserName,
				email = user.Email,
				
			};

			if (user.Profile != null) model.profile = MapProfileViewModel(user.Profile);

			if(!roles.IsNullOrEmpty()) model.roles= String.Join(",", roles.Select(r=>r.Name));


			return model;

		}

		public static ProfileViewModel MapProfileViewModel(Profile profile)
		{
			
			var model = new ProfileViewModel
			{
				fullname = profile.Fullname,
				sid=profile.SID,
				dob = profile.DOB,
				gender = profile.Gender

			};

			return model;

		}
	}
}
