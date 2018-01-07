using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public abstract class BaseAdminController : BlogWeb.Controllers.BaseController
	{
		public BaseAdminController(IHostingEnvironment environment, IOptions<AppSettings> settings) :base(environment, settings)
		{

		}

		protected string CurrentUserId
		{
			get { return Guid.NewGuid().ToString(); }
		}

	}
}
