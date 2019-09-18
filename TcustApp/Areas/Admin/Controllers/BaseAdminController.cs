using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace TcustApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	[Authorize(Policy = "ADMIN")]
	public abstract class BaseAdminController: TcustApp.Controllers.BaseController
	{
		public BaseAdminController(IHostingEnvironment environment, IOptions<AppSettings> settings)
		      :base(environment, settings)
		{
			

		}

		

	}
}
