using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityApp.Models;

namespace IdentityApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles ="Dev")]
	public class BaseAdminController : BaseController
	{
		public BaseAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
		   IHostingEnvironment environment, IOptions<AppSettings> settings) : base(userManager, roleManager, environment, settings)

		{
			
		}

	}
}