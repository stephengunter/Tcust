
using System.Collections.Generic;
using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ApplicationCore.Helpers;
using System;

namespace Blog.Models
{

	public class Post: BasePost
	{
		public string TermNumber { get; set; }

		public string Number { get; set; }

		public DateTime Date { get; set; }

		public string Author { get; set; }

		public int Year { get; set; }

		public int Month { get; set; }

		public bool Top { get; set; }

		public string Summary { get; set; }
		

		public ICollection<UploadFile> Attachments { get; set; }

		public ICollection<Click> Clicks { get; set; }



		[NotMapped]
		public ICollection<Category> Categories { get; }

		private ICollection<PostCategory> PostCategories { get; } = new List<PostCategory>();

		[NotMapped]
		public UploadFile TopFile
		{
			get
			{
				if (this.Attachments.IsNullOrEmpty()) return null;
				return this.Attachments.OrderByDescending(f => f.Order).FirstOrDefault();
			}
		}


		public Post()
		{
			CreatedAt = DateTime.Now;
			
			Year = DateTime.Now.Year;
			Month = DateTime.Now.Month;

			Categories = new JoinCollectionFacade<Category, Post, PostCategory>(this, PostCategories);
		}

	}

}
