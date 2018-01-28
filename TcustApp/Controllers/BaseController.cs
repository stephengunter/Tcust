using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;


namespace TcustApp.Controllers
{
    public abstract class BaseController  : ApplicationCore.Controllers.BaseController
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IOptions<AppSettings> settings;

		public BaseController(IHostingEnvironment environment, IOptions<AppSettings> settings)
		{
			this._hostingEnvironment = environment;
			this.settings = settings;

		}
    }
}