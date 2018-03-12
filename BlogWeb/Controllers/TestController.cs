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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BlogWeb.Controllers
{
	
	public class TestController : Controller
    {
		private readonly IHostingEnvironment _hostingEnvironment;

		public TestController(IHostingEnvironment environment)
		{
			this._hostingEnvironment = environment;
		}
		public IActionResult Index(int number=0)
		{

			return Content(Path.Combine(_hostingEnvironment.ContentRootPath, "Helpers"));
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
			var content = await client.GetStringAsync("http://localhost:50001/identity");

			ViewBag.Json = JArray.Parse(content).ToString();
			return View("json");
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