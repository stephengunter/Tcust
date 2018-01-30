using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Services;
using Blog.Helpers;
using Microsoft.Extensions.Options;
using WebApi.Controllers;
using Blog.Models;
using Blog.Views;

namespace WebApi.Blogs
{
    public class PostsController : BaseApiController
	{
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public PostsController(IOptions<Blog.Settings> settings, IPostService postService) 
		{

			this.postService = postService;

			this.viewService = new ViewService(settings, this.postService);
		}

		
		[HttpGet]
		public async Task<IActionResult> Index(int category = 0,  string keyword = "", int page = 1, int pageSize = 10)
		{
			bool reviewed = true;
			Category selectedCategory = null;
			if (category > 0) selectedCategory = await postService.GetCategoryByIdAsync(category);
			
			var posts = await postService.FetchPosts(selectedCategory, reviewed, keyword);

			posts = viewService.OrderPosts(posts);
			
			var pageList = await viewService.GetPostPagedList(posts, page, pageSize);

			return new ObjectResult(pageList);
		}


		[HttpGet("api/[controller]/[action]")]
		public async Task<IActionResult> GetDiaryList(string terms = "", string keyword = "", int page = 1, int pageSize = 10)
		{
			string code = "diary";
			Category diaryCategory = postService.GetCategoryByCode(code);
			

			bool reviewed = true;
			var posts = await postService.FetchPosts(diaryCategory, reviewed, keyword);

			if (!String.IsNullOrEmpty(terms))
			{
				var termNumbers = terms.Split(',').ToList().Select(int.Parse).ToList();
				posts = posts.Where(p => termNumbers.Contains(p.TermNumber));
			}

			posts = viewService.OrderPosts(posts);


			var pageList = await viewService.GetPostPagedList(posts, page, pageSize);

			return Ok(pageList);


		}

		[HttpGet("api/[controller]/[action]")] //校史館使用 GroupByYear
		public async Task<IEnumerable<PostsGroupByYearForm>> GetHonorList(string keyword = "")
		{
			var result = new List<PostsGroupByYearForm>();

			string code = "honor";
			Category diaryCategory = postService.GetCategoryByCode(code);


			bool reviewed = true;
			var posts = await postService.FetchPosts(diaryCategory, reviewed, keyword);

			var years = posts.Select(p => p.Year).Distinct();

			foreach (var year in years.OrderByDescending(y => y))
			{
				var model = new PostsGroupByYearForm(year);
				var postsInYear = posts.Where(p => p.Year == year)
								.OrderByDescending(p => p.Date).ThenByDescending(p => p.LastUpdated);


				foreach (var post in postsInYear)
				{
					
					model.posts.Add(viewService.MapPostViewModel(post));
				}

				result.Add(model);
			}

			

			return result;


		}

		[HttpGet("api/[controller]/[action]")] //校史館使用 GroupByYear
		public async Task<IActionResult> GetFamerList(string keyword = "", int page = 1, int pageSize=99)
		{

			string code = "famer";
			Category diaryCategory = postService.GetCategoryByCode(code);


			bool reviewed = true;
			var posts = await postService.FetchPosts(diaryCategory, reviewed, keyword);

			posts = posts.OrderByDescending(p => p.Date).ThenByDescending(p => p.LastUpdated);


			var pageList = await viewService.GetPostPagedList(posts, page, pageSize);

			return Ok(pageList);
		}

		[HttpGet("api/[controller]/[action]")] //校史館使用 
		public async Task<IActionResult> GetDaAiNews(string keyword = "", int year=0, int month=0, int page = 1, int pageSize = 99)
		{
			string code = "da-ai";
			Category diaryCategory = postService.GetCategoryByCode(code);


			bool reviewed = true;
			var posts = await postService.FetchPosts(diaryCategory, reviewed, keyword);

			if (year > 0) posts = posts.Where(p => p.Year == year);
			if (month > 0) posts = posts.Where(p => p.Month == month);

			posts = viewService.OrderPosts(posts);


			var pageList = await viewService.GetPostPagedList(posts, page, pageSize);

			return Ok(pageList);
		}

		[HttpGet("api/[controller]/[action]")] //校史館使用 
		public async Task<IEnumerable<int>> GetDaAiNewsYears()
		{
			string code = "da-ai";
			Category diaryCategory = postService.GetCategoryByCode(code);

			bool reviewed = true;
			var posts = await postService.FetchPosts(diaryCategory, reviewed);

			var years = posts.Select(p => p.Year).Distinct();

		
			return years;


		}


		public override async Task<IActionResult> Details(int id)
		{
			var post = postService.GetById(id);
			if (post == null) return NotFound();

			bool allMedias = true;

			var postViewModel = viewService.MapPostViewModel(post, allMedias);
			postViewModel.clickCount = await postService.GetPostClickCount(post.Id);

		
			return Ok(postViewModel);

		
		}
	}
}