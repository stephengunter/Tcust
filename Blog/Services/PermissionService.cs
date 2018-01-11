using System;
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Infrastructure.Data;

namespace Blog.Services
{
	public interface IPermissionService
	{
		bool IsUserHasPermission(string userName, string permissionName);
		//Task<IEnumerable<AppUser>> GetAllUserWithPermission();
	}

	public class PermissionService: IPermissionService
	{
		private readonly IBlogRepository<Permission> permissionRepository;
		private readonly IBlogRepository<AppUser> appUserRepository;

		private readonly IUserPermissionRepository userPermissionRepository;

		public PermissionService(IBlogRepository<Permission> permissionRepository, IBlogRepository<AppUser> appUserRepository, IUserPermissionRepository userPermissionRepository)
		{
			this.permissionRepository = permissionRepository;
			this.appUserRepository = appUserRepository;
			this.userPermissionRepository = userPermissionRepository;
		}

		public AppUser GetAppUser(string name)
		{
			var spec = new AppUserFilterSpecification(name);
			var user = appUserRepository.GetSingleBySpec(spec);

			return user;
		}

		public bool IsUserHasPermission(string userName, string permissionName)
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
