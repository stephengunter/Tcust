using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityApp.Models;
using IdentityApp.Models.AccountViewModels;
using IdentityApp.Services;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using IdentityApp.Data;
using IdentityApp.Helper;
using IdentityApp.Views;
using ApplicationCore.Helpers;

namespace IdentityApp.Controllers
{
    public class BaseController : ApplicationCore.Controllers.BaseController
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IHostingEnvironment environment;
		private readonly IOptions<AppSettings> settings;

		public BaseController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
			IHostingEnvironment environment, IOptions<AppSettings> settings)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.environment = environment;
			this.settings = settings;

		}

		protected UserManager<ApplicationUser> UserManager { get { return userManager; }  }
		protected RoleManager<IdentityRole> RoleManager { get { return roleManager; } }
		protected IHostingEnvironment Environment { get { return environment; } }
		protected IOptions<AppSettings> Settings { get { return settings; } }

		protected async Task<ApplicationUser> CreateUserAsync(string email, string role, Profile profile)
		{

			string username = email.GetUserNameFromEmail();
			string password = "000000";

			List<RoleType> roleTypes = new List<RoleType>();
			try
			{
				var roleType = (RoleType)Enum.Parse(typeof(RoleType), role);
				if (roleType == RoleType.Dev)
				{
					roleTypes.Add(RoleType.Dev);
					roleTypes.Add(RoleType.Staff);
				}
				else
				{

					roleTypes.Add(roleType);
				}
			}
			catch (Exception)
			{
				throw new Exception("RoleType Error. Role=" + role);
			}

			if (roleTypes.IsNullOrEmpty()) throw new Exception("RoleType Error. Role=" + role);


			var user = new ApplicationUser { UserName = username, Email = email };
			var result = await userManager.CreateAsync(user, password);

			if (!result.Succeeded) throw new Exception("Create User Failed. UserName= " + username);


			await AddToRolesAsync(user, roleTypes);

			user.CreatedAt = DateTime.Now;
			profile.CreatedAt = DateTime.Now;

			user.Profile = profile;

			await UserManager.UpdateAsync(user);

			return user;

		}

		

		protected async Task SyncUserRoles(ApplicationUser user, IList<string> roleNames)
		{

			var currentRoleNames = await userManager.GetRolesAsync(user);

			var needRemoveNames = currentRoleNames.Where(name => !roleNames.Contains(name));
			if (!needRemoveNames.IsNullOrEmpty())
			{
				await userManager.RemoveFromRolesAsync(user, needRemoveNames);
			}

			var needToAddNames = roleNames.Where(name => !currentRoleNames.Contains(name));
			if (!needToAddNames.IsNullOrEmpty())
			{
				await userManager.AddToRolesAsync(user, needToAddNames);
			}

		}

		public IEnumerable<IdentityRole> GetAllRoles()
		{
			return roleManager.Roles.ToList();
		}

		public Task<IdentityRole> GetRoleByNameAsync(string name)
		{
			return roleManager.FindByNameAsync(name);
		}

		public async Task<IEnumerable<IdentityRole>> GetRolesByUserAsync(ApplicationUser user)
		{
			var roleNames = await userManager.GetRolesAsync(user);
			return roleManager.Roles.Where(r => roleNames.Contains(r.Name));
		}

		async Task AddToRolesAsync(ApplicationUser user, List<RoleType> roleTypes)
		{
			foreach (var roleType in roleTypes)
			{
				string role = roleType.ToString();
				await userManager.AddToRoleAsync(user, role);
			}

		}

		
	}
}