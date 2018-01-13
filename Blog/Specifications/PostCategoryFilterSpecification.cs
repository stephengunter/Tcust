using ApplicationCore.Specifications;
using Blog.Models;
using ApplicationCore.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Blog.Specifications
{
	public class PostCategoryFilterSpecification : BaseSpecification<PostCategory>
	{
		public PostCategoryFilterSpecification(int postId, int categoryId)
		{
			Criteria = pc => pc.CategoryId == categoryId && pc.PostId == postId;
		}

		public PostCategoryFilterSpecification(Post post)
		{
			Criteria = pc => pc.PostId == post.Id;
		}

		public PostCategoryFilterSpecification(Category category)
		{
			Criteria = pc => pc.CategoryId == category.Id;
		}

		public PostCategoryFilterSpecification(Post post, IList<int> categoryIds)
		{

			Criteria = pc => pc.PostId == post.Id && categoryIds.Contains(pc.CategoryId);
		}
	}

}
