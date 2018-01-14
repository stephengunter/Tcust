
using Microsoft.EntityFrameworkCore;
using Permissions.Models;

namespace Permissions.DAL
{
	public class PermissionContext : DbContext
	{
		public PermissionContext(DbContextOptions<PermissionContext> options) : base(options)
		{
		}

		public PermissionContext(string connectionString) : base(new DbContextOptionsBuilder<PermissionContext>().UseSqlServer(connectionString).Options)
		{

		}

		

		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Permission> Permissons { get; set; }
		public DbSet<UserPermission> UserPermissions { get; set; }

		








	}
}
