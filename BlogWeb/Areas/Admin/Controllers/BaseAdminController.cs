using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using Permissions.Services;
using Microsoft.AspNetCore.Authorization;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public abstract class BaseAdminController : BlogWeb.Controllers.BaseController
	{
		protected readonly IPermissionService permissionService;

		public BaseAdminController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService) :base(environment, settings)
		{
			this.permissionService = permissionService;
		}

		protected bool CanReviewPost()
		{
			if (CurrentUserIsDev) return true;

			string permission = "REVIEW_POSTS";
			return permissionService.IsUserHasPermission(CurrentUserId, permission);
			
		}

	}
}
