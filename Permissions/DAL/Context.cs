
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			
			modelBuilder.Entity<UserPermission>(ConfigureUserPermission);

		}


		private void ConfigureUserPermission(EntityTypeBuilder<UserPermission> builder)
		{

			builder.HasKey(up => new { up.AppUserId, up.PermissionId });

			builder.HasOne(up => up.AppUser)
				.WithMany("UserPermissions")
				.HasForeignKey(up => up.AppUserId);


			builder.HasOne(up => up.Permission)
				.WithMany("UserPermissions")
				.HasForeignKey(up => up.PermissionId);


		}







	}
}
