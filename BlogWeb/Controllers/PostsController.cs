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

namespace BlogWeb.Controllers
{
    public class PostsController : BaseController
    {
		
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public PostsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPostService postService) : base(environment, settings)
		{

			this.postService = postService;

			this.viewService = new ViewService(this.Settings, this.postService);
		}

		[HttpGet]
		public async Task<IActionResult> Index(int category = 0,int year=0 , string keyword = "", int page = 1, int pageSize = 10)
		{
			Category selectedCategory = null;
			if (category > 0) selectedCategory = await postService.GetCategoryByIdAsync(category);
			if (selectedCategory == null) category = 0;

			bool reviewed = true;
			var posts = await postService.FetchPosts(selectedCategory, reviewed, keyword);
						

			if (!Request.IsAjaxRequest())
			{
				var archives = LoadArchives(posts);
				ViewData["archives"] = this.ToJsonString(archives);
			}

			int total = 0;
			if (!String.IsNullOrEmpty(keyword)) total = posts.Count();


			year = await postService.CheckYearAsync(year, posts);

			posts = posts.Where(p => p.Year == year);

			
			posts = posts.OrderByDescending(p => p.Date).ThenByDescending(p => p.LastUpdated);


			var pageList = await viewService.GetPostPagedList(posts, page, pageSize);
			


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			if (!String.IsNullOrEmpty(keyword)) pageList.TotalItems= total;


			bool excludeDefault = true;
			var categories = await postService.GetCategoriesAsync(excludeDefault);
		    

			var options = categories.Select(c => new { value = c.Id, text = c.Name });

			ViewData["category"] = category;
			ViewData["keyword"] = keyword;

			ViewData["categories"] = this.ToJsonString(options);
			

			ViewData["list"] = this.ToJsonString(pageList);

			return View();



			
		}

		
		[HttpGet("[controller]/{id}")]
		public async Task<IActionResult> Details(int id)
		{
			var post = postService.GetById(id);
			if (post == null) return NotFound();

			await postService.AddClick(id);

			bool allMedias = true;
			
			

			var postViewModel = viewService.MapPostViewModel(post, allMedias);
			postViewModel.clickCount = await postService.GetPostClickCount(post.Id);

			var model = new PostEditForm() {  post= postViewModel  };


			var categoryIds = await postService.GetCategoryIdsAsync(id);

			model.post.categoryId = categoryIds.FirstOrDefault();

			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(model);
			}

			ViewData["id"] = id;
			ViewData["model"] = this.ToJsonString(model);


			bool excludeDefault = true;
			var categories = await postService.GetCategoriesAsync(excludeDefault);

			var options = categories.Select(c => new { value = c.Id, text = c.Name });


			
			ViewData["category"] = model.post.categoryId;

			ViewData["categories"] = this.ToJsonString(options);

			return View("Index");
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