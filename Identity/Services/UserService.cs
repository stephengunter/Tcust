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
using IdentityApp.Helper;

namespace IdentityApp.Services
{
	public interface IUserService
	{
		Task<IEnumerable<ApplicationUser>> FetchUsers(IdentityRole role = null , string keyword = "");
	
		
		Task UpdateUserAsync(ApplicationUser user);

		ApplicationUser GetUserById(string id);
		ApplicationUser GetUserByEmail(string email);

		ApplicationUser GetUserByUserName(string username);

		Task <IEnumerable<IdentityRole>> GetAllRolesAsync();
		Task<IdentityRole> GetRoleByNameAsync(string name);

		Task<IEnumerable<IdentityRole>> GetRolesByUserIdAsync(string userId);

	}


	public class UserService: IUserService
	{
		private readonly IIdentityRepository<ApplicationUser> userRepository;
		private readonly IIdentityRepository<IdentityRole> roleRepository;
		private readonly IIdentityRepository<IdentityUserRole<string>> userRoleRepository;

		public UserService(IIdentityRepository<ApplicationUser> userRepository, IIdentityRepository<IdentityRole> roleRepository,
			IIdentityRepository<IdentityUserRole<string>> userRoleRepository)
		{
			this.userRepository = userRepository;
			this.roleRepository = roleRepository;
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

		

		public async Task UpdateUserAsync(ApplicationUser user)
		{
			user.LastUpdated= DateTime.Now;
			if (user.Profile != null)
			{
				user.Profile.LastUpdated = DateTime.Now;
			}
			
			await userRepository.UpdateAsync(user);

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

		public ApplicationUser GetUserByUserNameAsync(string username)
		{
			var spec = new UserNameFilterSpecifications(username);
			var user = userRepository.GetSingleBySpec(spec);

			return user;
		}


		public ApplicationUser GetUserByEmail(string email)
		{
			
			var spec = new UserEmailFilterSpecifications(email);
			var user = userRepository.GetSingleBySpec(spec);

			
			return user;
			
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

		public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
		{
			return await roleRepository.ListAllAsync();
		}

		public async  Task<IdentityRole> GetRoleByNameAsync(string name)
		{
			var all = await GetAllRolesAsync();
			return all.Where(r => r.Name == name).FirstOrDefault();
		}

		public async Task<IEnumerable<IdentityRole>> GetRolesByUserIdAsync(string userId)
		{
			var spec = new UserRoleFilterSpecifications(userId);
			var userRoles = await userRoleRepository.ListAsync(spec);

			var roleIds = userRoles.Select(ur => ur.RoleId);

			var allRoles = await GetAllRolesAsync();

			return allRoles.Where(r => roleIds.Contains(r.Id));
		}



	}
}
