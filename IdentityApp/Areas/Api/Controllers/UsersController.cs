using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Models;
using IdentityApp.Areas.Api.Models;

namespace IdentityApp.Areas.Api.Controllers
{

    public class UsersController : BaseApiController
    {
		private readonly UserManager<ApplicationUser> _userManager;

		public UsersController(UserManager<ApplicationUser> userManager)
		{
			_userManager= userManager;
		}

		[HttpGet]
		public async Task<IActionResult> test()
		{
			return Content("tgtg");
		}

		[HttpPost]
		public async Task<IActionResult> FindUserId([FromBody] FindUserModel model)
		{

			string userId = "";
			var user = await _userManager.FindByEmailAsync(model.email);

			if (user != null) userId = user.Id;

			return Content("test");
		}
	}
}