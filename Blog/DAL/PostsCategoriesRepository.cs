using Blog.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using ApplicationCore.Helpers;
using System.Linq;

namespace Blog.DAL
{
	public interface IPostsCategoriesRepository
	{
		IList<int> GetPostIds(int categoryId);

		IList<int> GetCategoryIds(int postId);

		void SyncPostCategories(int postId, IList<int> categoryIds);

		PostCategory Find(int postId, int categoryId);
	}

	public class PostsCategoriesRepository: BaseRepository<PostCategory> , IPostsCategoriesRepository
	{
		public PostsCategoriesRepository(BlogContext context) : base(context)
		{

		}

		public IList<int> GetPostIds(int categoryId)
		{
			return this.DbSet.Where(pc => pc.CategoryId == categoryId).Select(pc => pc.PostId).ToList();

		}

		public IList<int> GetCategoryIds(int postId)
		{
			return this.DbSet.Where(pc => pc.PostId == postId).Select(pc => pc.CategoryId).ToList();
		}

		public void SyncPostCategories(int postId, IList<int> categoryIds)
		{
			var current = GetCategoryIds(postId);

			var needRemove= current.Where(i => !categoryIds.Contains(i));
			if (!needRemove.IsNullOrEmpty())
			{
				foreach (var categoryId in needRemove)
				{
					Remove(postId, categoryId);
				}
			}

			var needToAdd= categoryIds.Where(i => !current.Contains(i));
			if (!needToAdd.IsNullOrEmpty())
			{
				foreach (var newCategoryId in needToAdd)
				{
					Add(postId, newCategoryId);
				}
			}

			this._dbContext.SaveChanges();

		}

		private void Add(int postId, int categoryId)
		{
			this.DbSet.Add(new PostCategory { PostId=postId, CategoryId=categoryId });
			
		}



		private void Remove(int postId, int categoryId)
		{
			this.DbSet.Remove(Find(postId, categoryId));
		}

		public PostCategory Find(int postId, int categoryId)
		{
			return this.DbSet.Find(postId, categoryId);
		}
	}
}
