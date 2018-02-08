using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace IdentityApp.Controllers
{
	public class News
	{
		public string number { get; set; }
		public int id { get; set; }
	}

	public class TestController : Controller
    {
		private readonly IOptions<AppSettings> settings;
		private readonly IHostingEnvironment _env;
		public TestController(IOptions<AppSettings> settings, IHostingEnvironment environment)
		{
			this.settings = settings;
			this._env = environment;
		}

		public IActionResult Index()
        {
			var lists = new List<News>
			{
				new News{  id=1  , number="news107009"},

				new News{  id=2  , number="news107018"},

				new News{  id=3  , number="news107110"},

			};

			lists = lists.OrderByDescending(n=>n.number).ToList();

			string test = "";
			for (int i = 0; i < lists.Count; i++)
			{
				test += lists[i].number + ",";
			}

			return Content(test);
			//return Content(GetForgotPasswordMailBody("阿水","http://google.com"));
        }

		private string GetForgotPasswordMailBody(string userName , string actionUrl)
		{
			string appName = "appName";
			string footer = "footer";
			var pathToFile = Path.Combine(_env.WebRootPath, "templates", "forgotPasswordEmail.html");
			if (!System.IO.File.Exists(pathToFile)) throw new Exception("File Not Exist: " + pathToFile);

			string body = "";
			using (StreamReader reader = System.IO.File.OpenText(pathToFile))
			{
				body = reader.ReadToEnd();
			}


			string messageBody = body.Replace("APPNAME", appName).Replace("USERNAME", userName).Replace("ACTION_URL", actionUrl).Replace("FOOTER", footer);

			return messageBody;
		}
	}
}