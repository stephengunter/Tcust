using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permissions.Models;

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
		public DbSet<Click> Clicks { get; set; }
		public DbSet<UploadFile> UploadFiles { get; set; }
		public DbSet<PostCategory> PostsCategories { get; set; }
		public DbSet<DepartmentTarget> DepartmentTargets { get; set; }

		public DbSet<PostIssuer> PostsIssuers { get; set; }
		public DbSet<PostDepartment> PostsDepartments { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<PostCategory>(ConfigurePostCategory);
			modelBuilder.Entity<PostIssuer>(ConfigurePostIssuer);
			modelBuilder.Entity<PostDepartment>(ConfigurePostDepartment);

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

		private void ConfigurePostIssuer(EntityTypeBuilder<PostIssuer> builder)
		{

			builder.HasKey(pc => new { pc.PostId, pc.DepartmentId });


		}

		private void ConfigurePostDepartment(EntityTypeBuilder<PostDepartment> builder)
		{

			builder.HasKey(pc => new { pc.PostId, pc.DepartmentId });


		}






	}
}
