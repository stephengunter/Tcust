using System.Collections.Generic;
using IdentityApp.Data;
using IdentityApp.Models;
using IdentityApp.Specifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Views;
using ApplicationCore.Helpers;

namespace IdentityApp.Services
{
	public interface IUserService
	{
		Task<IEnumerable<ApplicationUser>> FetchUsers(IdentityRole role = null , string keyword = "");
		Task<ApplicationUser> CreateUserAsync(string username, string role, Profile profile);
		
		Task UpdateUserAsync(ApplicationUser user);
		Task UpdateUserAsync(ApplicationUser user, IList<string> roles);

		ApplicationUser GetUserById(string id);
		ApplicationUser GetUserByEmail(string email);

		ApplicationUser GetUserByUserName(string username);
		Task<ApplicationUser> GetUserByUserNameAsync(string username);

		IEnumerable<IdentityRole> GetAllRoles();
		Task<IdentityRole> GetRoleByNameAsync(string name);

		Task<IEnumerable<IdentityRole>> GetRolesByUserAsync(ApplicationUser user);
		Task<IEnumerable<IdentityRole>> GetRolesByUserIdAsync(string userId);

	}


	public class UserService: IUserService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		private readonly IIdentityRepository<ApplicationUser> userRepository;
		private readonly IIdentityRepository<IdentityUserRole<string>> userRoleRepository;

		public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
			IIdentityRepository<ApplicationUser> userRepository, IIdentityRepository<IdentityUserRole<string>> userRoleRepository)

		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.userRepository = userRepository;
			this.userRoleRepository = userRoleRepository;
		}

		public async Task<IEnumerable<ApplicationUser>> FetchUsers(IdentityRole role=null ,string keyword = "")
		{
			Task<IEnumerable<ApplicationUser>> getUsersTask;
			if (String.IsNullOrEmpty(keyword))
			{
				getUsersTask = GetAllAsync();
			}
			else
			{
				getUsersTask = GetByKeywordAsync(keyword);
			}

			var users = await getUsersTask;

			if (role != null)
			{
				
				var idsInRole = await GetUserIdsInRoleAsync(role);
				users = users.Where(p => idsInRole.Contains(p.Id));

			}

			
			return users;
		}

		public async Task<ApplicationUser> CreateUserAsync(string username ,string role, Profile profile)
		{
			string email = String.Format("{0}@tcust.edu.tw", username);
			string password = "secret";

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

			if(roleTypes.IsNullOrEmpty()) throw new Exception("RoleType Error. Role=" + role);


			var user = new ApplicationUser { UserName = username, Email = email };
			var result = await userManager.CreateAsync(user, password);

			if (!result.Succeeded) throw new Exception("Create User Failed. UserName= " + username);


			await AddToRolesAsync(user, roleTypes);

			user.CreatedAt = DateTime.Now;
			profile.CreatedAt = DateTime.Now;

			user.Profile = profile;

			await UpdateUserAsync(user);

			return user;

		}

		public async Task UpdateUserAsync(ApplicationUser user)
		{
			user.Email = String.Format("{0}@tcust.edu.tw", user.UserName);

			user.LastUpdated= DateTime.Now;
			if (user.Profile != null)
			{
				user.Profile.LastUpdated = DateTime.Now;
			}
			
			await userRepository.UpdateAsync(user);

		}
		public async Task UpdateUserAsync(ApplicationUser user, IList<string> roles)
		{
			await SyncUserRoles(user, roles);
			await UpdateUserAsync(user);
		}

		public ApplicationUser GetUserById(string id)
		{
			var spec = new UserIdFilterSpecifications(id);
			var user = userRepository.GetSingleBySpec(spec);


			return user;
		}

		public ApplicationUser  GetUserByUserName(string username)
		{
			
			var spec = new UserNameFilterSpecifications(username);
			var user = userRepository.GetSingleBySpec(spec);

			return user;
		}

		public async Task<ApplicationUser> GetUserByUserNameAsync(string username)
		{
			return await userManager.FindByNameAsync(username);
		}


		public ApplicationUser GetUserByEmail(string email)
		{
			
			var spec = new UserEmailFilterSpecifications(email);
			var user = userRepository.GetSingleBySpec(spec);

			
			return user;
			
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

		public async Task<IEnumerable<IdentityRole>> GetRolesByUserIdAsync(string userId)
		{
			var spec = new UserRoleFilterSpecifications(userId);
			var userRoles = await userRoleRepository.ListAsync(spec);

			var roleIds = userRoles.Select(ur => ur.RoleId);

			return roleManager.Roles.Where(r => roleIds.Contains(r.Id));
		}

		async Task AddToRolesAsync(ApplicationUser user, List<RoleType> roleTypes)
		{
			foreach (var roleType in roleTypes)
			{
				string role = roleType.ToString();
				await userManager.AddToRoleAsync(user, role);
			}
			
		}

		async Task<IEnumerable<ApplicationUser>> GetAllAsync()
		{
			var filter = new BaseUserFilterSpecifications();
			return await userRepository.ListAsync(filter);
		}

		async Task<IEnumerable<ApplicationUser>> GetByKeywordAsync(string keyword)
		{
			var filter = new UserKeywordFilterSpecifications(keyword);

			return await userRepository.ListAsync(filter);
		}

		async Task<IList<string>> GetUserIdsInRoleAsync(IdentityRole role)
		{
			var spec = new UserRoleFilterSpecifications(role);
			var userRoles = await userRoleRepository.ListAsync(spec);

			return userRoles.Select(ur => ur.UserId).ToList();


		}

		async Task SyncUserRoles(ApplicationUser user, IList<string> roleNames)
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
	}
}
