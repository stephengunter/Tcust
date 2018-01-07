using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;

namespace BlogWeb.Controllers
{
	public abstract class BaseController : Controller
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




	}
}