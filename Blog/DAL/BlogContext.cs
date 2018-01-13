using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationCore.Entities;

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

		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Permisson> Permissons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<PostCategory>(ConfigurePostCategory);

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

		






	}
}
