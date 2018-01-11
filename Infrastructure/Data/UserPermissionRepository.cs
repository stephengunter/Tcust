using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ApplicationCore.Helpers;

namespace Infrastructure.Data
{
	public interface IUserPermissionRepository
	{
		IList<int> GetAppUserIds(int permissionId);

		IList<int> GetPermissionIds(int appUserId);

		void SyncAppUserPermissions(int appUserId, IList<int> permissionIds);

		UserPermission Find(int appUserId, int permissionId);
	}

	public class UserPermissionRepository : BaseRepository<UserPermission>, IUserPermissionRepository
	{
		public UserPermissionRepository(DbContext context) : base(context)
		{

		}

		public IList<int> GetAppUserIds(int permissionId)
		{
			return this.DbSet.Where(pc => pc.PermissionId == permissionId).Select(pc => pc.AppUserId).ToList();

		}

		public IList<int> GetPermissionIds(int appUserId)
		{
			return this.DbSet.Where(pc => pc.AppUserId == appUserId).Select(pc => pc.PermissionId).ToList();
		}

		public void SyncAppUserPermissions(int appUserId, IList<int> permissionIds)
		{
			var current = GetPermissionIds(appUserId);

			var needRemove = current.Where(i => !permissionIds.Contains(i));
			if (!needRemove.IsNullOrEmpty())
			{
				foreach (var permissionId in needRemove)
				{
					Remove(appUserId, permissionId);
				}
			}

			var needToAdd = permissionIds.Where(i => !current.Contains(i));
			if (!needToAdd.IsNullOrEmpty())
			{
				foreach (var newpermissionId in needToAdd)
				{
					Add(appUserId, newpermissionId);
				}
			}

			this._dbContext.SaveChanges();

		}

		private void Add(int appUserId, int permissionId)
		{
			this.DbSet.Add(new UserPermission { AppUserId = appUserId, PermissionId = permissionId });

		}



		private void Remove(int appUserId, int permissionId)
		{
			this.DbSet.Remove(Find(appUserId, permissionId));
		}

		public UserPermission Find(int appUserId, int permissionId)
		{
			return this.DbSet.Find(appUserId, permissionId);
		}
	}
}
