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
	
	public class PostViewModel
	{
		public PostViewModel()
		{

		}
		

		//public PostViewModel(Post post, bool allMedias=false)
		//{
		//	this.id = post.Id;
		//	this.title = post.Title;
		//	this.author = post.Author;
		//	this.content = post.Content;
		//	this.createdAt = post.CreatedAt;

		//	if (String.IsNullOrEmpty(post.Summary)) this.summary = PostViewModel.GetDefaultSummary(post);
		//	else this.summary = post.Summary;

		//	//this.medias = new List<MediaViewModel>();
		//	//foreach (var media in post.Attachments)
		//	//{
		//	//	this.medias.Add(new MediaViewModel(media));
		//	//}

		//	if (allMedias)
		//	{
		//		this.medias = new List<MediaViewModel>();
		//		foreach (var media in post.Attachments)
		//		{
		//			this.medias.Add(new MediaViewModel(media));
		//		}
		//	}

		//	if(post.TopFile!=null) this.cover = new MediaViewModel(post.TopFile);



		//}

		public int id { get; set; }

		[Required(ErrorMessage = "請填寫標題")]
		public string title { get; set; }

		[Required(ErrorMessage = "請填寫學年標題")]
		public string termNumber { get; set; }

		public string number { get; set; }
		public string author { get; set; }

		[Required(ErrorMessage = "請填寫內容")]
		public string content { get; set; }

		public string summary { get; set; }

		public string date { get; set; }


		public DateTime createdAt { get; set; }

		public int categoryId { get; set; }



		public MediaViewModel cover { get; set; }

		public List<MediaViewModel> medias { get; set; }

		public static string GetDefaultSummary(Post post)
		{
			string str = post.Content.RemoveHtmlTags().Trim();
			return str.Substring(0, Math.Min(str.Length, 100)) + " ...";
		}


		public Post MapToEntity(string updatedBy , Post post=null)
		{
			if (post == null)
			{
				post = new Post();
				post.Attachments = new List<UploadFile>();
			}


			post.Title = title;
			post.Content = content;
			post.TermNumber = termNumber.Trim();

			post.Author = author;

			post.Number = number;


			post.Date = date.ToDatetimeOrDefault(DateTime.Now);

			post.SetUpdated(updatedBy);



			return post;
		}



		//copy專用

		public DateTime updatedAt { get; set; }
		public string fileIds { get; set; }

		public string categoryName { get; set; }

	}

	public class PostEditForm
	{
		public PostViewModel post { get; set; }
	}









}
