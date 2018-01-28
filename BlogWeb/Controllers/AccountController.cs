using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace BlogWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

		public async Task Logout()
		{
			await HttpContext.SignOutAsync("Cookies");
			await HttpContext.SignOutAsync("oidc");
		}
	}
}