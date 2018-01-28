using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;

namespace TcustApp.Areas.Admin.Controllers
{
    public class TermsController : BaseAdminController
	{
		public TermsController(IHostingEnvironment environment, IOptions<AppSettings> settings)
		      :base(environment, settings)
		{


		}

		public IActionResult Index()
        {
            return View();
        }
    }
}