using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;

namespace BlogWeb.Controllers
{
	public abstract class BaseController : ApplicationCore.Controllers.BaseController
	{
		

		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IOptions<AppSettings> settings;


		public BaseController(IHostingEnvironment environment, IOptions<AppSettings> settings)
		{
			this._hostingEnvironment = environment;
			this.settings = settings;
			
		}

		protected IOptions<AppSettings> Settings { get { return this.settings; } }

		protected string UploadFilesPath => Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

		protected string HelpersPath => Path.Combine(_hostingEnvironment.WebRootPath, "helpers");



		protected string DefaultTermNumber()
		{
			var year = DateTime.Now.Year - 1911 -1;
			if (DateTime.Now.Month > 7) return year.ToString() + 2;
			return year.ToString() + 1;

		}

		

		


	}
}