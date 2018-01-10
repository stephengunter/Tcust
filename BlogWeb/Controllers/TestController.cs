using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;

namespace BlogWeb.Controllers
{
	
	public class TestController : Controller
    {
		public TestController()
		{

		}
		public IActionResult Index()
		{
			var id = (from p in HttpContext.User.Claims where p.Type == "sub" select p.Value).First();
		
			return Content(User.IsInRole("Dev").ToString());
		}

		[Authorize]
		public IActionResult Secure()
		{
			ViewData["Message"] = "Secure page.";

			return View();
		}

		public async Task Logout()
		{
			await HttpContext.SignOutAsync("Cookies");
			await HttpContext.SignOutAsync("oidc");
		}

		public IActionResult Error()
		{
			return View();
		}

		public async Task<IActionResult> CallApiUsingClientCredentials()
		{
			var tokenClient = new TokenClient("http://localhost:50000/connect/token", "mvc", "secret");
			var tokenResponse = await tokenClient.RequestClientCredentialsAsync("apiApp");

			var client = new HttpClient();
			client.SetBearerToken(tokenResponse.AccessToken);
			var content = await client.GetStringAsync("http://localhost:50001/api/identity");

			ViewBag.Json = JArray.Parse(content).ToString();
			return View("json");
		}

		public async Task<IActionResult> CallApiUsingUserAccessToken()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			var client = new HttpClient();
			client.SetBearerToken(accessToken);
			var content = await client.GetStringAsync("http://localhost:50001/api/identity");

			ViewBag.Json = JArray.Parse(content).ToString();
			return View("json");
		}
	}
}