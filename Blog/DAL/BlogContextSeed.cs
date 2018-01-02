using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.DAL
{
	public class BlogContextSeed
	{
		public static void Seed(BlogContext context)
		{
			
		}

		static void SeedPostsWithAttachments(BlogContext context)
		{
			var postList = GetPosts();
			foreach (var item in postList)
			{
				context.Posts.Add(item);
			}

			context.SaveChanges();
		}

		static List<Models.Post> GetPosts()
		{
			string connectionString = @"data source=.\SQLEXPRESS;initial catalog=TCUST_History;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

			var postList = new List<Models.Post>();

			using (var oldPostContext = new Blog.DAL.OldPosts.OldPostContext(connectionString))
			{
				var oldPostList = oldPostContext.Posts.Include(p => p.UploadFiles).Take(100);

				foreach (var oldPost in oldPostList)
				{
					var post = new Models.Post()
					{
						Author = oldPost.Author,
						Content = oldPost.Content,
						Summary = oldPost.Summary,
						DisplayOrder = oldPost.DisplayOrder,
						CreatedAt = oldPost.CreatedAt,
						CreateMonth = oldPost.CreateMonth,
						CreateYear = oldPost.CreateYear,
						Down = oldPost.Down,
						LastUpdated = oldPost.UpdatedAt,
						Title = oldPost.Title,
						Top = oldPost.Top

					};

					post.Attachments = new List<Models.UploadFile>();
					foreach (var item in oldPost.UploadFiles)
					{
						post.Attachments.Add(new Models.UploadFile
						{
							CreatedAt = post.CreatedAt,
							Height = item.Height,
							Name = item.Name,
							LastUpdated = post.LastUpdated,
							Path = item.Path,
							PreviewPath = item.PreviewPath,
							PS = item.PS,
						
							Type = item.Type,
							UpdatedBy = post.UpdatedBy,
							Width = item.Width,

						

						});
					}

					postList.Add(post);

				}

				

			}

			return postList;
		}

		static void SeedCategories(BlogContext context)
		{
			string name = "榮譽榜";
			string code = "honor";
			var exist = context.Categories.Where(c => c.Code == code).FirstOrDefault();
			if (exist == null)
			{
				context.Categories.Add(new Blog.Models.Category
				{
					Code = code,
					Name = name,
					Active = true
				});

				context.SaveChanges();
			}

		}
	}

	
}
