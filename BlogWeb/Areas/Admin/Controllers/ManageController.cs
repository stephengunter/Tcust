using Blog.Services;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using ApplicationCore.Paging;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using Permissions.Services;
using Permissions.Models;
using Permissions.Views;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace BlogWeb.Areas.Admin.Controllers
{
	//[Authorize(Policy = "MANAGE_USERS")]
	public class ManageController : BaseAdminController
	{
		private readonly IPermissionService permissionService;

		public ManageController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService) : base(environment, settings)
		{
			this.permissionService = permissionService;
		}

		public async Task<IActionResult> Index(int permission = 0, string keyword = "", int page = 1, int pageSize = 10)
		{
			
			Permission selectedPermission = null;
			if (permission > 0) selectedPermission = await permissionService.GetPermissionByIdAsync(permission);
			if (selectedPermission == null) permission = 0;

			var users = await permissionService.FetchUsersWithPermissions(selectedPermission, keyword);

			users = users.OrderByDescending(u => u.LastUpdated);

			var pageList = new PagedList<AppUser, UserViewModel>(users, page, pageSize);

			foreach (var user in pageList.List)
			{
				var permissions =await permissionService.GetUserPermissionsAsync(user);
				
				pageList.ViewList.Add(PermissionViewService.MapUserViewModel(user, permissions.ToList()));
			}

			pageList.List = null;


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			string token = await GetToken();
			ViewData["token"] = token;

			bool edit = false;
			var permissionsOptions = await permissionService.GetPermissionOptionsAsync(edit);

			ViewData["permissions"] = this.ToJsonString(permissionsOptions);


			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}
		

		[HttpGet]
		public async Task<IActionResult> Create()
		{

			var model = new UserEditForm
			{
				user = new UserViewModel() 
			};

			bool edit = true;
			bool isDev = CurrentUserIsDev;
			var permissionOptions = await permissionService.GetPermissionOptionsAsync(edit, isDev);
			model.permissionOptions= permissionOptions.ToList();

			return new ObjectResult(model);
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] UserEditForm model)
		{
			
			var exist = permissionService.GetAppUserByUserId(model.user.userId);
			if(exist!=null) ModelState.AddModelError("user.userId", "這個User重複了");


			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = model.user.MapToEntity(CurrentUserId);
			user.CreatedAt = DateTime.Now;
			user.SetUpdated(CurrentUserId);

			var permissionIds = model.user.permissionIds;
			
			foreach (var permissionId in permissionIds)
			{
				var permission = await permissionService.GetPermissionByIdAsync(permissionId);
				if (permission.AdminOnly && !CurrentUserIsDev)
				{
					throw new Exception("無權授予此權限. Permission=" + permission.Name);
				}

				user.Permissions.Add(permission);

			}

			user = await permissionService.CreateAppUserAsync(user);


			return new ObjectResult(user);


		}
	}
}