using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BlogWeb.Controllers
{
	[Authorize]
	public class TestController : Controller
    {
        public async Task<IActionResult> Index()
        {
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			var client = new HttpClient();
			client.SetBearerToken(accessToken);
			var content = await client.GetStringAsync("http://localhost:50001/api/identity");

			ViewBag.Json = JArray.Parse(content).ToString();


			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync("Cookies");
			await HttpContext.SignOutAsync("oidc");

			return Redirect("/");
		}

		public async Task<IActionResult> CallApiUsingUserAccessToken()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			var client = new HttpClient();
			client.SetBearerToken(accessToken);
			var content = await client.GetStringAsync("http://localhost:50001/identity");

			ViewBag.Json = JArray.Parse(content).ToString();
			return View("json");
		}
	}
}