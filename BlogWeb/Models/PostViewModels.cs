using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blog.Models;
using ApplicationCore.Helpers;
using ApplicationCore.Paging;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
		public PostViewModel()
		{
			this.medias = new List<MediaViewModel>();
		}

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
			
			//this.cover = new MediaViewModel(post.TopFile);

		}

		public int id { get; set; }

		[Required(ErrorMessage = "請填寫標題")]
		public string title { get; set; }

		
		public string author { get; set; }

		[Required(ErrorMessage = "請填寫內容")]
		public string content { get; set; }

		public string summary { get; set; }

		public string date { get; set; }


		public DateTime createdAt { get; set; }

		public MediaViewModel cover { get; set; }

		public List<MediaViewModel> medias { get; set; }

		public static string GetDefaultSummary(Post post)
		{
			string str = post.Content.RemoveHtmlTags().Trim();
			return str.Substring(0, Math.Min(str.Length, 100)) + " ...";
		}


		public Post MapToEntity(string updatedBy)
		{
			var post = new Post();
			post.Attachments = new List<UploadFile>();

			post.Title = title.Trim();
			post.Content = content.Trim();
			post.Author =author.Trim();

			post.Date = date.ToDatetimeOrDefault(DateTime.Now);

			post.SetUpdated(updatedBy);



			return post;
		}

	}

	public class PostEditForm
	{
		public PostViewModel post { get; set; }
	}









}
