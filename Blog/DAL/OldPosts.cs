using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.DAL.OldPosts
{
	public class OldPostContext : DbContext
	{
		public OldPostContext(string connectionString) : base(new DbContextOptionsBuilder<OldPostContext>().UseSqlServer(connectionString).Options)
		{

		}


		public DbSet<Post> Posts { get; set; }
		public DbSet<UploadFile> UploadFiles { get; set; }
	}

	public class Post
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public string Author { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public int CreateYear { get; set; }

		public int CreateMonth { get; set; }

		public string ContentId { get; set; }

		public bool Down { get; set; }

		public bool Top { get; set; }

		public string Summary { get; set; }

		public int DisplayOrder { get; set; }

		public virtual ICollection<UploadFile> UploadFiles { get; set; }

	}

	public class UploadFile
	{
		public int Id { get; set; }

		public int PostId { get; set; }

		public string ItemOID { get; set; }

		public string Path { get; set; }

		public string PreviewPath { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string Type { get; set; }

		public string Name { get; set; }

		public string PS { get; set; }

		public bool Top { get; set; }


	}
}
