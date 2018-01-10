using System;
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;

namespace Blog.Services
{
	public interface IPermissionService
	{
		bool IsUserHasPermission(string userId, string permissionName);
	}

	public class PermissionService: IPermissionService
	{
		private readonly IBlogRepository<Permission> permissionRepository;
		private readonly IBlogRepository<UserPermission> userPermissionRepository;

		public PermissionService(IBlogRepository<Permission> permissionRepository, IBlogRepository<UserPermission> userPermissionRepository)
		{
			this.permissionRepository = permissionRepository;
			this.userPermissionRepository = userPermissionRepository;
		}

		public bool IsUserHasPermission(string userId, string permissionName)
		{
			var spec = new PermissionFilterSpecification(permissionName);
			var permission = permissionRepository.GetSingleBySpec(spec);

			if (permission == null) throw new Exception("Permission Not Found. PermissionName= " + permissionName);


			var userPermissionFilter = new UserPermissionFilterSpecification(userId, permission.Id);

			var userPermission = userPermissionRepository.GetSingleBySpec(userPermissionFilter);

			if (userPermission == null) return false;
			return true;

		}
	}
}
