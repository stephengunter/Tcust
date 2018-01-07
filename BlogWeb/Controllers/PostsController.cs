using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using Blog.Models;
using Blog.Services;
using ApplicationCore.Helpers;
using BlogWeb.Models;
using ApplicationCore.Paging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using BlogWeb.Helpers;

namespace BlogWeb.Controllers
{
    public class PostsController : BaseController
    {
		
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public PostsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPostService postService) : base(environment, settings)
		{

			this.postService = postService;

			this.viewService = new ViewService(this.Settings);
		}

		[HttpGet]
		public async Task<IActionResult> Index(int category = 0,int year=0 , string keyword = "", int page = 1, int pageSize = 10)
		{
			
			var posts = await postService.FetchPosts(category, keyword);

			if (!Request.IsAjaxRequest())
			{
				var archives = LoadArchives(posts);
				ViewData["archives"] = this.ToJsonString(archives);
			}



			year = await postService.CheckYearAsync(year, posts);

			posts = posts.Where(p => p.Year == year);

			//Order
			posts = posts.OrderByDescending(p => p.Date).ThenByDescending(p => p.LastUpdated);



			var pageList = new PagedList<Post, PostViewModel>(posts, page, pageSize);

			foreach (var item in pageList.List)
			{
				pageList.ViewList.Add(viewService.MapPostViewModel(item));
			}

			pageList.List = null;


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			var categories = await postService.GetCategoriesAsync();

			var options = categories.Select(c => new { value = c.Id, text = c.Name });

			ViewData["category"] = category;
			ViewData["keyword"] = keyword;

			ViewData["categories"] = this.ToJsonString(options);
			

			ViewData["list"] = this.ToJsonString(pageList);

			return View();



			
		}

		
		[HttpGet("[controller]/{id}")]
		public IActionResult Details(int id)
		{
			var post = postService.GetById(id);
			if (post == null) return NotFound();

			bool allMedias = true;
			var model = new PostEditForm
			{
				post = viewService.MapPostViewModel(post, allMedias)
			};

			model.post.categoryId = postService.GetCategoryIds(id).FirstOrDefault();

			return new ObjectResult(model);
		}



		private List<MenuItem> LoadArchives(IEnumerable<Post> posts )
		{
			var menuItems = new List<MenuItem>();

			if (posts.IsNullOrEmpty()) return menuItems;
			

			var years = posts.GroupBy(p => new { Year = p.Year })
						.Select(d => new { Year = d.Key.Year, count = d.Count() })
						.OrderByDescending(g => g.Year);

			foreach (var singleYear in years)
			{
				int year = singleYear.Year;
				int count = singleYear.count;
				var item = new MenuItem();
				item.Text = year.ToString();
				item.Count = count;

				menuItems.Add(item);
			}

			return menuItems;

		}

	}
}