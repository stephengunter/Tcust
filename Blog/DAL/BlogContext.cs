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

		public DbSet<Blog.Models.Category> Categories { get; set; }
		public DbSet<Blog.Models.Post> Posts { get; set; }
		public DbSet<Blog.Models.UploadFile> UploadFiles { get; set; }

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
