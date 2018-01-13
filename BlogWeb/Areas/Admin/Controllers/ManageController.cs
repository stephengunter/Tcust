using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Blog.Models;
using Blog.Services;
using ApplicationCore.Helpers;
using BlogWeb.Models;
using ApplicationCore.Paging;

using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using BlogWeb.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace BlogWeb.Areas.Admin.Controllers
{
	//[Authorize(Policy = "MANAGE_USERS")]
	public class ManageController : BaseAdminController
	{
		

		public ManageController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
		{
			
		}

		public IActionResult Index(int pageSize = 10)
        {
			//var userPermissions = await permissionService.GetAllUserWithPermission();


			return View();
        }
    }
}