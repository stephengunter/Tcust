using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DAL
{


	public class BlogContext : DbContext
	{
		public BlogContext(DbContextOptions<BlogContext> options) : base(options)
		{
		}

		public BlogContext(string connectionString):base(new DbContextOptionsBuilder<BlogContext>().UseSqlServer(connectionString).Options)
		{
			
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<UploadFile> UploadFiles { get; set; }
		public DbSet<PostCategory> PostsCategories { get; set; }

		public DbSet<ApplicationCore.Entities.Permission> Permissions { get; set; }
		public DbSet<ApplicationCore.Entities.AppUser> AppUsers { get; set; }
		public DbSet<ApplicationCore.Entities.UserPermission> UserPermissions { get; set; }
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<PostCategory>(ConfigurePostCategory);

			modelBuilder.Entity<ApplicationCore.Entities.UserPermission>(ConfigureUserPermission);

		}

		private void ConfigurePostCategory(EntityTypeBuilder<PostCategory> builder)
		{

			builder.HasKey(pc => new { pc.PostId, pc.CategoryId });

			builder.HasOne(pc => pc.Post)
				.WithMany("PostCategories")
				.HasForeignKey(pc => pc.PostId);


			builder.HasOne(pc => pc.Category)
				.WithMany("PostCategories")
				.HasForeignKey(pc => pc.CategoryId);
			

		}

		private void ConfigureUserPermission(EntityTypeBuilder<ApplicationCore.Entities.UserPermission> builder)
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
