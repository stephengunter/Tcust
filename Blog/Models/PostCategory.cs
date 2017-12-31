using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Models
{
    public class PostCategory : IJoinEntity<Post>, IJoinEntity<Category>
	{
		public int PostId { get; set; }
		public Post Post { get; set; }
		Post IJoinEntity<Post>.Navigation
		{
			get => Post;
			set => Post = value;
		}

		public int CategoryId { get; set; }
		public Category Category { get; set; }
		Category IJoinEntity<Category>.Navigation
		{
			get => Category;
			set => Category = value;
		}


	}
}
