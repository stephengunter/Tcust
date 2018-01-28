
using System.Collections.Generic;
using ApplicationCore.Entities;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Helpers;

namespace Blog.Models
{

	public class Category: BaseCategory
	{
		public Category() => Posts = new JoinCollectionFacade<Post, Category, PostCategory>(this, PostCategories);


		[NotMapped]
		public ICollection<Post> Posts { get; }

		private ICollection<PostCategory> PostCategories { get; } = new List<PostCategory>();
		
	}
}
