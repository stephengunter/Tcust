using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace History.DAL.Tccn
{
    public class TccnContext : DbContext
	{
		public TccnContext(DbContextOptions<TccnContext> options) : base(options)
		{
		}

		public TccnContext(string connectionString):base(new DbContextOptionsBuilder<TccnContext>().UseSqlServer(connectionString).Options)
		{

		}

		public DbSet<Content> Contents { get; set; }
		public DbSet<FileUplaod> FileUplaods { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Educated> Educateds { get; set; }
		
		public DbSet<ContentMultipleType> ContentMultipleTypes { get; set; }
	}
	[Table("Content")]
	public class Content
	{
		[Key]
		public string ContentID { get; set; }
		public string TypeID { get; set; }
		public string SerialNo { get; set; }
		public DateTime? ContentTime { get; set; }
		public string ContentName { get; set; }
		public string Description { get; set; }
		public bool? IsPublish { get; set; }
		public bool? ShowDate { get; set; }
		public DateTime? OpenTime { get; set; }
		public DateTime? CloseTime { get; set; }
		public string RelatedLink { get; set; }
		public string ContentCreator { get; set; }
		public DateTime? ContentCreateTime { get; set; }
		public string ContentUpdater { get; set; }
		public DateTime? ContentUpdateTime { get; set; }
		public string ContentText { get; set; }
		public string Author { get; set; }
		public int DisplayOrder { get; set; }

		public ICollection<FileUplaod> FileUplaods { get; set; }
	}
	[Table("FileUplaod")]
	public partial class FileUplaod
	{
		[Key]
		public string ItemOID { get; set; }
		
		public string FunctionOID { get; set; }
		public string FunctionType { get; set; }
		public string ItemName { get; set; }
		public string ItemInfo { get; set; }
		public string FileName { get; set; }
		public string Path { get; set; }
		public string Bit { get; set; }
		public int? Sort { get; set; }
		public int? ImageWidth { get; set; }
		public int? ImageHeight { get; set; }
		public string Creator { get; set; }
		public DateTime? CreateTime { get; set; }
		public long? ClickCount { get; set; }
		public bool CoverImage { get; set; }
		public string PreviewPath { get; set; }

		public string ContentID { get; set; }
		public virtual Content Content { get; set; }
	}
	[Table("ContentMultipleType")]
	public class ContentMultipleType
	{
		[Key]
		public string ContentID { get; set; }
		public string ContentType { get; set; }
	}

	[Table("Category")]
	public partial class Category
	{
		public string CategoryID { get; set; }
		public string CategoryTypeID { get; set; }
		public string CategoryName { get; set; }
		public int? Sort { get; set; }
		public DateTime? CreateTime { get; set; }
		public string Creator { get; set; }
		public DateTime? UpdateTime { get; set; }
		public string Updater { get; set; }
	}
	[Table("Educated")]
	public partial class Educated
	{
		[Key]
		public string ContentID { get; set; }
		public string CategoryYearID { get; set; }
		public string CategoryTermID { get; set; }
		public string CategoryEducatedID { get; set; }
		public string CategoryDoingID { get; set; }
	}
}
