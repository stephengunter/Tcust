
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
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace TcustApp.Areas.Admin.Controllers
{
	
	public class ManageController : BaseAdminController
	{
		private readonly IPermissionService permissionService;
		public ManageController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService) : base(environment, settings)
		{
			this.permissionService = permissionService;
		}

		Permission GetAdminPermission()
		{
			var permission = permissionService.GetPermissionByName("ADMIN");
			if (permission == null) throw new Exception("Permission not found. name = ADMIN");
			return permission;
		}

		public async Task<IActionResult> Index(string keyword = "", int page = 1, int pageSize = 10)
		{
			var permission = GetAdminPermission();

			var users = await permissionService.FetchUsersWithPermission(permission, keyword);

			users = users.OrderByDescending(u => u.LastUpdated);

			var pageList = new PagedList<AppUser, UserViewModel>(users, page, pageSize);

			foreach (var user in pageList.List)
			{
				var permissions = await permissionService.GetUserPermissionsAsync(user);

				pageList.ViewList.Add(PermissionViewService.MapUserViewModel(user, permissions.ToList()));
			}

			pageList.List = null;


			if (Request.IsAjaxRequest())
			{
				return Ok(pageList);
			}

			string token = await GetToken();
			ViewData["token"] = token;

			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}


		[HttpGet]
		public IActionResult Create()
		{

			var model = new UserEditForm
			{
				user = new UserViewModel() 
			};
			
			return Ok(model);
		}

		
		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] UserEditForm model)
		{
			
			var exist = permissionService.GetAppUserByUserId(model.user.userId);
			if(exist != null) ModelState.AddModelError("user.userId", "這個User重複了");


			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = model.user.MapToEntity(CurrentUserId);
			user.CreatedAt = DateTime.Now;
			user.SetUpdated(CurrentUserId);

			var permission = GetAdminPermission();
			user.Permissions.Add(permission);

			user = await permissionService.CreateAppUserAsync(user);


			return Ok(user);


		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
			var user =await permissionService.GetAppUserByIdAsync(id);
			if (user == null) return NotFound();

			var model = new UserEditForm();
			
			var userModel = PermissionViewService.MapUserViewModel(user);
			

			model.user = userModel;

			return new ObjectResult(model);
		}
		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UserEditForm model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await permissionService.GetAppUserByIdAsync(id);
			if (user == null) return NotFound();

			user = model.user.MapToEntity(CurrentUserId, user);

			var permission = GetAdminPermission();
			

			await permissionService.UpdateAppUserAsync(user, new List<int> { permission.Id });


			return new NoContentResult();


		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			await permissionService.DeleteAppUserAsync(id);

			return new NoContentResult();

		}

	}
}