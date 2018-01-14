using System;
using System.Collections.Generic;
using System.Text;
using Permissions.Models;
using System.Threading.Tasks;
using System.Linq;
using Permissions.DAL;
using Permissions.Specifications;

namespace Permissions.Services
{
	public interface IPermissionService
	{
		bool IsUserHasPermission(string userId, string permissionName);
		Task<IEnumerable<AppUser>> FetchUsersWithPermissions(Permission permission = null, string keyword = "");

		Task<Permission> GetPermissionByIdAsync(int id);
		Task<IEnumerable<Permission>> GetPermissionsAsync();

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

		public async Task<Permission> GetPermissionByIdAsync(int id)
		{
			return await permissionRepository.GetByIdAsync(id);
		}

		public async Task<IEnumerable<AppUser>> FetchUsersWithPermissions(Permission permission = null, string keyword = "")
		{
			Task<IEnumerable<AppUser>> getAppUsersTask;

			if (String.IsNullOrEmpty(keyword))
			{
				getAppUsersTask = GetAllUsersAsync();
			}
			else
			{
				getAppUsersTask = GetByKeywordAsync(keyword);
			}



			var users = await getAppUsersTask;


			if (permission != null)
			{
				var userIdsInPermission = await GetUserIdsInPermission(permission);

				return users.Where(u => userIdsInPermission.Contains(u.Id));


			}

			return users;

		}

		public async Task<IEnumerable<Permission>> GetPermissionsAsync()
		{
			var spec = new BasePermissionFilterSpecification();
			return await permissionRepository.ListAsync(spec);
		}

		private async Task<IEnumerable<AppUser>> GetAllUsersAsync()
		{
			var filter = new BaseAppUserFilterSpecification();
			return await appUserRepository.ListAsync(filter);

		}

		private async Task<IEnumerable<AppUser>> GetByKeywordAsync(string keyword)
		{
			var filter = new AppUserFilterSpecification(keyword);

			return await appUserRepository.ListAsync(filter);
		}
		private async Task<IList<int>> GetUserIdsInPermission(Permission permission)
		{
			var filter = new UserPermissionFilterSpecification(permission);

			var userPermissions = await userPermissionRepository.ListAsync(filter);

			return userPermissions.Select(up => up.UserId).ToList();


		}


		public AppUser GetAppUser(string name)
		{
			var spec = new AppUserFilterSpecification(name);
			var user = appUserRepository.GetSingleBySpec(spec);

			return user;
		}

		public bool IsUserHasPermission(string userId, string permissionName)
		{
			return true;
			//var 

			//var spec = new PermissionFilterSpecification(permissionName);
			//var permission = permissionRepository.GetSingleBySpec(spec);

			//if (permission == null) throw new Exception("Permission Not Found. PermissionName= " + permissionName);


			//var userPermissionFilter = new UserPermissionFilterSpecification(userName, permission.Id);

			//var userPermission = userPermissionRepository.GetSingleBySpec(userPermissionFilter);

			//if (userPermission == null) return false;
			//return true;

		}

		//public async Task<IEnumerable<UserPermission>> GetAllUserWithPermission()
		//{
		//	var spec = new UserPermissionFilterSpecification();
		//	return await userPermissionRepository.ListAsync(spec);
		//}
	}
}
