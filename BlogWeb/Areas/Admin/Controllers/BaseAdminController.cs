using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public abstract class BaseAdminController : BlogWeb.Controllers.BaseController
	{
		private readonly IHostingEnvironment _hostingEnvironment;

		public BaseAdminController(IHostingEnvironment environment)
		{
			this._hostingEnvironment = environment;
		}

		protected string UploadFilesPath
		{
			get {  return Path.Combine(_hostingEnvironment.WebRootPath, "uploads"); }
		}

	}
}
