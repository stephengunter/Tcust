using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Controllers;
using IdentityApp.Services;
using IdentityApp.Views;

namespace WebApi.Identity
{

	public class UsersController : BaseApiController
	{
		private readonly IUserService userService;

		public UsersController(IUserService userService)
		{
			this.userService = userService;
		}

		
		public IActionResult GetUserByEmail(string email)
		{
			var user= userService.GetUserByEmail(email);

			if (user == null) return NotFound();

			var model = IdentityViewService.MapUserViewModel(user);
			

			return new ObjectResult(model);
		}


		

	}
}