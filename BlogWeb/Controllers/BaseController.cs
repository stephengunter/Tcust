using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;

namespace BlogWeb.Controllers
{
	public abstract class BaseController : ApplicationCore.Controllers.BaseController
	{
		private readonly JsonSerializerSettings jsonSettings;

		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IOptions<AppSettings> settings;


		public BaseController(IHostingEnvironment environment, IOptions<AppSettings> settings)
		{
			this._hostingEnvironment = environment;
			this.settings = settings;

			this.jsonSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			};
		}

		protected IOptions<AppSettings> Settings { get { return this.settings; } }

		protected string UploadFilesPath
		{
			get { return Path.Combine(_hostingEnvironment.WebRootPath, "uploads"); }
		}

		protected string HelpersPath
		{
			get { return Path.Combine(_hostingEnvironment.ContentRootPath, "Helpers"); }
		}

		protected string ToJsonString(object model)
		{
			return JsonConvert.SerializeObject(model, this.jsonSettings);
		}

		protected string DefaultTermNumber()
		{
			var year = DateTime.Now.Year - 1911 -1;
			if (DateTime.Now.Month > 7) return year.ToString() + 2;
			return year.ToString() + 1;

		}

		protected async Task<string> GetToken()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			return accessToken;
		}

		


	}
}