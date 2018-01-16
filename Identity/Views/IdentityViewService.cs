using System;
using System.Collections.Generic;
using System.Text;
using IdentityApp.Models;

namespace IdentityApp.Views
{
    public class IdentityViewService
    {
		public static IdentityUserViewModel MapUserViewModel(ApplicationUser user)
		{
			var model = new IdentityUserViewModel
			{
				 email= user.Email,
				 id=user.Id
			};

			if (user.Profile != null)
			{
				model.fullname = user.Profile.Fullname;
			}

			return model;
		}
	}
}
