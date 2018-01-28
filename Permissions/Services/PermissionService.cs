using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using System.Threading.Tasks;
using System.Linq;
using Permissions.DAL;
using Permissions.Specifications;
using Permissions.Views;
using ApplicationCore.Helpers;

namespace Permissions.Services
{
	public interface IPermissionService
	{
		Task<AppUser> GetAppUserByIdAsync(int id);
		AppUser GetAppUserByUserId(string userId);
		Task<IEnumerable<AppUser>> FetchUsersWithPermissions(Permission permission = null, string keyword = "");

		Task<AppUser> CreateAppUserAsync(AppUser user);
		Task UpdateAppUserAsync(AppUser user, IList<int> permissionIds);
		Task DeleteAppUserAsync(int id);


		bool IsUserHasPermission(string userId, string permissionName);

		Task<IEnumerable<Permission>> GetUserPermissionsAsync(AppUser user);

		Task<Permission> GetPermissionByIdAsync(int id);


		Task<List<PermissionOption>> GetPermissionOptionsAsync(bool edit, bool isDev = false);

	}

	public class PermissionService : IPermissionService
	{
		private readonly IPermissionRepository<Permission> permissionRepository;
		private readonly IPermissionRepository<AppUser> appUserRepository;
		private readonly IPermissionRepository<UserPermission> userPermissionRepository;

		public PermissionService(IPermissionRepository<Permission> permissionRepository
			, IPermissionRepository<AppUser> appUserRepository, IPermissionRepository<UserPermission> userPermissionRepository)
		{
			this.permissionRepository = permissionRepository;
			this.appUserRepository = appUserRepository;
			this.userPermissionRepository = userPermissionRepository;
		}

		public async Task<AppUser> GetAppUserByIdAsync(int id)
		{
			return await appUserRepository.GetByIdAsync(id);
		}
		public AppUser GetAppUserByUserId(string userId)
		{
			return appUserRepository.Get(u => u.UserId == userId);
		}

		public async Task<Permission> GetPermissionByIdAsync(int id)
		{
			return await permissionRepository.GetByIdAsync(id);
		}

		public async Task<IEnumerable<AppUser>> FetchUsersWithPermissions(Permission permission = null, string keyword = "")
		{
			Task<IEnumerable<AppUser>> getAppUsersTask;

			if (String.IsNullOrEmpty(keyword))
			{
				getAppUsersTask = GetAllAppUsersAsync();
			}
			else
			{
				getAppUsersTask = GetAppUserByKeywordAsync(keyword);
			}



			var users = await getAppUsersTask;


			if (permission != null)
			{
				var userIdsInPermission = await GetUserIdsInPermission(permission);

				return users.Where(u => userIdsInPermission.Contains(u.Id));


			}

			return users;

		}

		public async Task<List<PermissionOption>> GetPermissionOptionsAsync(bool edit, bool isDev=false)
		{
			var permissions = await permissionRepository.ListAllAsync();
			permissions = permissions.OrderByDescending(p => p.Order).ToList();

			if (edit)
			{
				if (!isDev) permissions = permissions.Where(p => !p.AdminOnly).ToList();  
			}

			var options = new List<PermissionOption>();
			foreach (var item in permissions)
			{
				options.Add(PermissionViewService.MapPermissionOption(item));
			}

			return options;
		}

		public async Task<AppUser> CreateAppUserAsync(AppUser user)
		{
			return await appUserRepository.AddAsync(user);

	    }
		public async Task UpdateAppUserAsync(AppUser user, IList<int> permissionIds)
		{
			await appUserRepository.UpdateAsync(user);

			await SyncUserPermissions(user, permissionIds);
		}

		public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(AppUser user)
		{
			var permissionIds =await GetPermissionIdsInUser(user);

			var filter = new PermissionFilterSpecification(permissionIds);
			return  permissionRepository.List(filter).OrderByDescending(p=>p.Order);
			
		}
		public async Task DeleteAppUserAsync(int id)
		{
			var user = await appUserRepository.GetByIdAsync(id);
			await appUserRepository.DeleteAsync(user);

		}

		private async Task<IEnumerable<AppUser>> GetAllAppUsersAsync()
		{
			return await appUserRepository.ListAllAsync();

		}

		private async Task<IEnumerable<AppUser>> GetAppUserByKeywordAsync(string keyword)
		{

			var filter = new AppUserFilterSpecification(keyword);

			return await appUserRepository.ListAsync(filter);
		}


		private async Task<IList<int>> GetUserIdsInPermission(Permission permission)
		{
			var filter = new UserPermissionFilterSpecification(permission);

			var userPermissions = await userPermissionRepository.ListAsync(filter);

			return userPermissions.Select(up => up.AppUserId).ToList();


		}
		private async Task SyncUserPermissions(AppUser user, IList<int> permissionIds)
		{
			var currentPermissionIds = await GetPermissionIdsInUser(user);

			var needRemoveIds = currentPermissionIds.Where(i => !permissionIds.Contains(i));
			if (!needRemoveIds.IsNullOrEmpty())
			{
				var spec = new UserPermissionFilterSpecification(user, needRemoveIds.ToList());
				var removeItems = await userPermissionRepository.ListAsync(spec);

				userPermissionRepository.DeleteRange(removeItems);
			}

			var needToAdd = permissionIds.Where(i => !currentPermissionIds.Contains(i));
			if (!needToAdd.IsNullOrEmpty())
			{
				foreach (var newPermissionId in needToAdd)
				{
					await userPermissionRepository.AddAsync(new UserPermission { AppUserId = user.Id, PermissionId = newPermissionId });

				}
			}

		}
		//private async Task SyncUserPermissions(AppUser user, IEnumerable<Permission> permissions)
		//{
		//var currentPermissions = permissionRepository.
		//var currentPermissions=

		//var current = await GetCategoryIdsInPost(post);

		//var needRemoveIds = current.Where(i => !categoryIds.Contains(i));
		//if (!needRemoveIds.IsNullOrEmpty())
		//{
		//	var spec = new PostCategoryFilterSpecification(post, needRemoveIds.ToList());
		//	var removeItems = await postsCategoriesRepository.ListAsync(spec);

		//	postsCategoriesRepository.DeleteRange(removeItems);
		//}

		//var needToAdd = categoryIds.Where(i => !current.Contains(i));
		//if (!needToAdd.IsNullOrEmpty())
		//{
		//	foreach (var newCategoryId in needToAdd)
		//	{
		//		await postsCategoriesRepository.AddAsync(new PostCategory { PostId = post.Id, CategoryId = newCategoryId });

		//	}
		//}

		//}


		//public AppUser GetAppUser(string name)
		//{
		//	var spec = new AppUserFilterSpecification(name);
		//	var user = appUserRepository.GetSingleBySpec(spec);

		//	return user;
		//}

		public bool IsUserHasPermission(string userId, string permissionName)
		{
			if (String.IsNullOrEmpty(userId)) return false;


			var appUser = GetAppUserByUserId(userId);
			if(appUser==null) throw new Exception("AppUser Not Found. UserId= " + userId);

			var spec = new PermissionFilterSpecification(permissionName);
			var permission = permissionRepository.GetSingleBySpec(spec);

			if (permission == null) throw new Exception("Permission Not Found. PermissionName= " + permissionName);

			
			var userPermissionFilter = new UserPermissionFilterSpecification(appUser, permission);
			var userPermission = userPermissionRepository.GetSingleBySpec(userPermissionFilter);

			return (userPermission != null) ;
			

		}

		private async Task<IList<int>> GetPermissionIdsInUser(AppUser user)
		{
			var filter = new UserPermissionFilterSpecification(user);

			var userPermissions = await userPermissionRepository.ListAsync(filter);

			return userPermissions.Select(up => up.PermissionId).ToList();
		}
	}
}
