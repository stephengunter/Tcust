using System;
using System.Collections.Generic;

using Blog.Models;
using ApplicationCore.Helpers;
using System.ComponentModel.DataAnnotations;

using ApplicationCore.Views;

namespace Blog.Views
{

	public class PostViewModel
	{
		public PostViewModel()
		{
			categoryIds = new List<int>();
		}

		public int id { get; set; }

		[Required(ErrorMessage = "請填寫標題")]
		public string title { get; set; }

		[Required(ErrorMessage = "請填寫學年標題")]
		public int termNumber { get; set; }

		public string number { get; set; }
		public string author { get; set; }

		[Required(ErrorMessage = "請填寫內容")]
		public string content { get; set; }

		public string summary { get; set; }

		public string date { get; set; }
		public string beginDate { get; set; }
		public string endDate { get; set; }

		public bool top { get; set; }

		public int order { get; set; }

		public bool reviewed { get; set; }


		public DateTime createdAt { get; set; }

		public IList<int> categoryIds { get; set; }

		public string categoryNames { get; set; }

		public string url { get; set; }

		public int clickCount { get; set; }

		public MediaViewModel cover { get; set; }

		public List<MediaViewModel> medias { get; set; }



		public static string GetDefaultSummary(Post post)
		{
			string str = post.Content.RemoveHtmlTags().Trim();
			return str.Substring(0, Math.Min(str.Length, 160)) + " ...";
		}


		public Post MapToEntity(string updatedBy, Post post = null)
		{
			if (post == null)
			{
				post = new Post();
				post.Attachments = new List<UploadFile>();
			}


			post.Title = title;
			post.Content = content;
			post.TermNumber = termNumber;

			post.Author = author;

			post.Top = top;
			post.DisplayOrder = order;

			post.Reviewed = reviewed;
			post.Number = number;

			if (String.IsNullOrEmpty(beginDate))
			{
				post.BeginDate = DateTime.Now;
				post.EndDate = null;

			}
			else
			{
				post.BeginDate = beginDate.ToDatetimeOrDefault(DateTime.Now);

				if (String.IsNullOrEmpty(endDate)) post.EndDate = null;
				else post.EndDate = endDate.ToDatetimeOrNull();

			}

			post.Date =Convert.ToDateTime(post.BeginDate);

			post.Year = post.Date.Year;
			post.Month = post.Date.Month;

			post.SetUpdated(updatedBy);



			return post;
		}



		//copy專用

		public DateTime updatedAt { get; set; }
		public string fileIds { get; set; }



	}

	public class PostEditForm
	{
		public int canReview { get; set; }

		public IList<int> issuerIds { get; set; }

		public IList<int> departmentIds { get; set; }

		public PostViewModel post { get; set; }

		public List<BaseOption> categoryOptions { get; set; }
	}

	public class PostReviewForm
	{
		public IList<int> postIds { get; set; }
	}

	public class TopPostEditForm
	{
		public int id { get; set; }
		public int order { get; set; }

	}


	public class PostsGroupByYearForm
	{
		public PostsGroupByYearForm(int year)
		{
			this.year = year;
			this.posts = new List<PostViewModel>();
		}

		public int year { get; set; }
		public List<PostViewModel> posts { get; set; }
	}









}
