
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
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using IdentityApp.Views;
using System.Net.Mail;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Authorize(Policy = "MANAGE_USERS")]
	public class ManageController : BaseAdminController
	{
		public ManageController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService) : base(environment, settings, permissionService)
		{
			
		}

		AppSettings AppSettings => Settings.Value;

		public async Task<IActionResult> Index(int permission = 0, string keyword = "", int page = 1, int pageSize = 10)
		{
			Permission selectedPermission = null;
			if (permission > 0) selectedPermission = await permissionService.GetPermissionByIdAsync(permission);
			if (selectedPermission == null) permission = 0;

			var users = await permissionService.FetchUsersWithPermission(selectedPermission, keyword);

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
			var permissionsOptions = await GetPermissionOptions(edit);

			permissionsOptions.Insert(0, new PermissionOption { text = "----------", value = 0 });

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
			model.permissionOptions=await GetPermissionOptions(edit);

			return new ObjectResult(model);
		}

		private async Task<List<PermissionOption>> GetPermissionOptions(bool edit)
		{
			bool isDev = CurrentUserIsDev;
			var permissionOptions = await permissionService.GetPermissionOptionsAsync(edit, isDev);
			return permissionOptions.ToList();
		}

		async Task<string> PostResponseStringAsync(string action, object model)
		{
			var client = new HttpClient
			{
				BaseAddress = new Uri(AppSettings.AuthUrl)
			};
			string json = JsonConvert.SerializeObject(model);

			HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");


			var response = await client.PostAsync(action, contentPost);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();

		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] UserEditForm model)
		{
			if (String.IsNullOrEmpty(AppSettings.AdminKey))
			{
				ModelState.AddModelError("adminKey", "找不到adminKey");
				return BadRequest(ModelState);
			}

			ValidateRequest(model);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var userId = await PostResponseStringAsync($"api/users/{AppSettings.AdminKey}", new IdentityUserViewModel{ email = model.user.email, name = model.user.name });

			model.user.userId = userId;

			var existUser = permissionService.GetAppUserByUserId(model.user.userId);
			if (existUser == null)
			{
				//新增
				var user = model.user.MapToEntity(CurrentUserId);
				user.CreatedAt = DateTime.Now;
				user.SetUpdated(CurrentUserId);
				var permissions = await GetPermissionsToGrant(model.user.permissionIds);
				foreach (var item in permissions)
				{
					user.Permissions.Add(item);
				}

				user = await permissionService.CreateAppUserAsync(user);
				return Ok();

			}
			else
			{
				//更新
				existUser = model.user.MapToEntity(CurrentUserId, existUser);


				await permissionService.UpdateAppUserAsync(existUser, model.user.permissionIds);


				return Ok();
			}

		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
			var user =await permissionService.GetAppUserByIdAsync(id);
			if (user == null) return NotFound();

			var model = new UserEditForm();
			bool edit = true;
			model.permissionOptions = await GetPermissionOptions(edit);

			var permissions = await permissionService.GetUserPermissionsAsync(user);

			var userModel = PermissionViewService.MapUserViewModel(user, permissions.ToList());
			userModel.permissionIds = permissions.Select(p => p.Id).ToArray();

			

			model.user = userModel;

			return new ObjectResult(model);
		}
		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UserEditForm model)
		{
			if (String.IsNullOrEmpty(AppSettings.AdminKey))
			{
				ModelState.AddModelError("adminKey", "找不到adminKey");
				return BadRequest(ModelState);
			}

			ValidateRequest(model);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var userId = await PostResponseStringAsync($"api/users/{AppSettings.AdminKey}", new IdentityUserViewModel { email = model.user.email, name = model.user.name });

			model.user.userId = userId;

			var user = await permissionService.GetAppUserByIdAsync(id);
			if (user == null) return NotFound();

			user = model.user.MapToEntity(CurrentUserId, user);

			await permissionService.UpdateAppUserAsync(user, model.user.permissionIds);


			return Ok();

		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			await permissionService.DeleteAppUserAsync(id);

			return new NoContentResult();

		}

		private async Task<List<Permission>>  GetPermissionsToGrant(IList<int> permissionIds)
		{
			var permissions = new List<Permission>();
			foreach (var permissionId in permissionIds)
			{
				var permission = await permissionService.GetPermissionByIdAsync(permissionId);
				if (permission.AdminOnly && !CurrentUserIsDev)
				{
					throw new Exception("無權授予此權限. Permission=" + permission.Name);
				}

				permissions.Add(permission);

			}

			return permissions;
		}

		void ValidateRequest(UserEditForm model)
		{
			if (String.IsNullOrEmpty(model.user.email))
			{
				ModelState.AddModelError("user.email", "必須填寫Email");
			}
			else
			{
				try
				{
					MailAddress m = new MailAddress(model.user.email);
				}
				catch (FormatException)
				{
					ModelState.AddModelError("user.email", "email格式錯誤");
				}
			}

			

			if (String.IsNullOrEmpty(model.user.name))
			{
				ModelState.AddModelError("user.name", "必須填寫姓名");
			}

			if (model.user.permissionIds.IsNullOrEmpty())
			{
				ModelState.AddModelError("user.permissionIds", "必須選擇權限");
			}


			
		}
	}
}