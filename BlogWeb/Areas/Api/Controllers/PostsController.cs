using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using Blog.Models;
using Blog.Services;
using ApplicationCore.Helpers;
using Blog.Views;
using ApplicationCore.Paging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Blog.Helpers;
using Microsoft.AspNetCore.Cors;

namespace BlogWeb.Areas.Api.Controllers
{
	[Area("Api")]
	public class PostsController : BlogWeb.Controllers.BaseController
	{
		private readonly IPostService postService;

		public PostsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPostService postService) : base(environment, settings)
		{

			this.postService = postService;
		}

		//public async Task<IActionResult> Index()
  //      {
		//	var selectedCategory = await postService.GetCategoryByIdAsync(0, true);

		//	bool reviewed = true;
		//	var posts = await postService.FetchPosts(selectedCategory, reviewed, "");

		//	posts = posts.Where(p => p.Year >= 2013).OrderByDescending(p => p.Top)
		//				.ThenByDescending(p => p.DisplayOrder)
		//				.ThenByDescending(p => p.Date)
		//				.ThenByDescending(p => p.LastUpdated)
		//				.Take(16); 




			
		//}
    }


	public class PostViewModel
	{
		public PostViewModel()
		{
		}

		public PostViewModel(Post post)
		{
			this.Id = post.Id;
			this.Title = post.Title;
			this.Content = post.Content;
			this.CreatedAt = post.CreatedAt;
			this.Url = string.Format("{0}/Posts/Details?id={1}&title={2}", "http://blog.tcust.edu.tw", post.Id, post.Title);
			this.Summary = !string.IsNullOrEmpty(post.Summary) ? post.Summary : this.GetDefaultSummary(post.Content);

			if (post.Attachments.IsNullOrEmpty()) return;

			this.MediaViewModels = new List<BlogWeb.Areas.Api.Controllers.MediaViewModel>();
			var cover = post.Attachments.OrderByDescending(f => f.Order).FirstOrDefault();
			this.MediaViewModels.Add(new BlogWeb.Areas.Api.Controllers.MediaViewModel(cover ,true));
		}

		public string GetDefaultSummary(string content)
		{
			string str = content.RemoveHtmlTags().Trim();
			return str.Substring(0, Math.Min(str.Length, 160));
		}

		public int Id { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public string Summary { get; set; }

		public string Url { get; set; }

		public DateTime CreatedAt { get; set; }

		public virtual ICollection<MediaViewModel> MediaViewModels { get; set; }
	}

	public class MediaViewModel
	{
		public MediaViewModel()
		{
		}

		public MediaViewModel(UploadFile file,bool top)
		{
			this.Id = file.Id;
			this.Width = file.Width;
			this.Height = file.Height;
			this.Name = file.Name;
			this.Path = file.Path;
			this.Top = top;
		}

		public int Id { get; set; }

		public int PostId { get; set; }

		public string ItemOID { get; set; }

		public string Path { get; set; }

		public string Name { get; set; }

		public bool Top { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }
	}
}