using Blog.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Blog.DAL
{
	public interface IPostsCategoriesRepository
	{
		IList<int> GetPostIds(int categoryId);
	}

	public class PostsCategoriesRepository: BaseRepository<PostCategory> , IPostsCategoriesRepository
	{
		public PostsCategoriesRepository(BlogContext context) : base(context)
		{

		}

		public IList<int> GetPostIds(int categoryId)
		{
			return this.DbSet.Where(p => p.CategoryId == categoryId).Select(p => p.PostId).ToList();
		}
    }
}
