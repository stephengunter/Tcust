using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityApp.Models;
using IdentityApp.Models.ManageViewModels;
using IdentityApp.Services;
using IdentityApp.Helper;
using ApplicationCore.Helpers;
using IdentityApp.Views;
using ApplicationCore.Views;
using Microsoft.AspNetCore.Hosting;

namespace IdentityApp.Areas.Admin.Controllers
{

	public class UsersController : BaseAdminController
	{
		
		private readonly IUserService userService;

		public UsersController(UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> roleManager,
			IHostingEnvironment environment, IOptions<AppSettings> settings, IUserService userService)
			: base(UserManager, roleManager, environment, settings)
		{
			this.userService = userService;
		}



		public async Task<IActionResult> Index(string role = "", string keyword = "", int page = 1, int pageSize = 10)
		{
			IdentityRole selectedRole = null;
			if (!String.IsNullOrEmpty(role)) selectedRole = await userService.GetRoleByNameAsync(role);
			if (selectedRole == null) role = "";

			var users = await userService.FetchUsers(selectedRole, keyword);

			users = users.GetOrdered();

			var pageList = users.GetUsersPagedList(page , pageSize);

			foreach (var item in pageList.ViewList)
			{
				var roles = await userService.GetRolesByUserIdAsync(item.id);
				item.roles = String.Join(",", roles.Select(r => r.Name));
			}


			if (Request.IsAjaxRequest())
			{
				return Ok(pageList);
			}

			bool edit = false;
			var roleOptions = GetRoleOptions(edit);

			ViewData["roles"] = this.ToJsonString(roleOptions);

			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{

			var userModel = new IdentityUserViewModel() { profile=new ProfileViewModel() };

			var model = new IdentityUserEditViewModel { user = userModel , roleOptions = GetRoleOptions()};

			model.roles = new string[] { "Staff" };

			return Ok(model);
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] IdentityUserEditViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			string email = model.user.email.Trim();
			string role = model.roles.FirstOrDefault();

			var exist =  userService.GetUserByEmail(email);

			if (exist != null)
			{
				ModelState.AddModelError("user.email", "Email重複了");
				return BadRequest(ModelState);
			}

			
			var profile = new Profile
			{
				Fullname = model.user.profile.fullname,
			};

			var user= await CreateUserAsync(email, role, profile);

			return Ok(user);


		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(string id)
		{
			var user = userService.GetUserById(id);
			if (user == null) return NotFound();

			var userModel = user.MapIdentityUserViewModel();

			var model = new IdentityUserEditViewModel { user = userModel, roleOptions = GetRoleOptions() };

			var roles = await GetRolesByUserAsync(user);
			model.roles = roles.Select(r => r.Name).ToArray();

			return Ok(model);
			
		}

		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] IdentityUserEditViewModel model)
		{
			var user = userService.GetUserById(id);
			if (user == null) return NotFound();

			string email = model.user.email;

			var exist = userService.GetUserByEmail(email);
			if (exist != null && exist.Id != id)
			{
				ModelState.AddModelError("user.email", "Email重複了");
				return BadRequest(ModelState);
			}

			user = model.user.MapToEntity(user);

			await userService.UpdateUserAsync(user);

			await SyncUserRoles(user, model.roles);

			return Ok(user);


		}


		List<BaseOption> GetRoleOptions(bool edit=true)
		{
			bool emptyOption = !edit;
			
			var roles = GetAllRoles();
			return roles.ToOptions(emptyOption);
		}

		

	}
}