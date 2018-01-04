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

namespace BlogWeb.Controllers
{
    public class PostsController : BaseController
    {
		private readonly int pagesize = 8;
		private readonly IPostService postService;

		public PostsController(IPostService postService)
		{
			this.postService = postService;
		}

		public async Task<IActionResult> Index()
		{
			int year = 0;
			int page = 1;
			

			var allPost = await postService.GetAllAsync();

			year = await postService.CheckYearAsync(year, allPost);

			var yearPosts = allPost;// await postService.GetByYearAsync(year, allPost);


			var pageList = new PagedList<Post, PostViewModel>(yearPosts, page, this.pagesize);

			foreach (var item in pageList.List)
			{
				pageList.ViewList.Add(new PostViewModel(item));
			}

			pageList.List = null;


			var model = new PostSearchModel();
			model.PagedList = pageList;

			ViewBag.list = this.ToJsonString(model.PagedList);

			return View();
		}

	}
}