using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blog.Models;
using ApplicationCore.Helpers;
using ApplicationCore.Paging;
using Microsoft.AspNetCore.Http;

namespace BlogWeb.Models
{
	public class PostSearchModel 
	{
		//public string Keyword { get; set; }
		//public int Year { get; set; }
		//public int Month { get; set; }
		

		//public int PageNumber { get; set; }
		//public int PageSize { get; set; }

		//public string CurrentUrl { get; set; }

		public PagedList<Post, PostViewModel> PagedList  { get; set; }

		public IEnumerable<MenuItem> MenuItems { get; set; }

		//public Post Post { get; set; }
	}

	

	public class PostViewModel
	{
		public PostViewModel() { }

		public PostViewModel(Post post, bool allMedias=false)
		{
			this.id = post.Id;
			this.title = post.Title;
			this.author = post.Author;
			this.content = post.Content;
			this.createdAt = post.CreatedAt;

			if (String.IsNullOrEmpty(post.Summary)) this.summary = PostViewModel.GetDefaultSummary(post);
			else this.summary = post.Summary;

			if (allMedias)
			{
				this.medias = new List<MediaViewModel>();
				foreach (var media in post.Attachments)
				{
					this.medias.Add(new MediaViewModel(media));
				}
			}
			
			this.cover = new MediaViewModel(post.TopFile);

		}

		public int id { get; set; }
		public string title { get; set; }
		public string author { get; set; }
		public string content { get; set; }
		public string summary { get; set; }
		public DateTime createdAt { get; set; }

		public MediaViewModel cover { get; set; }

		public List<MediaViewModel> medias { get; set; }

		public static string GetDefaultSummary(Post post)
		{
			string str = post.Content.RemoveHtmlTags().Trim();
			return str.Substring(0, Math.Min(str.Length, 100)) + " ...";
		}

	}


	

	

	
}
