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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using BlogWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Views;
using Permissions.Services;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Authorize(Policy = "REVIEW_POSTS")]
	public class ReviewController : BaseAdminController
	{
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public ReviewController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService, IPostService postService) 
			: base(environment, settings, permissionService)
		{

			this.postService = postService;

			this.viewService = new ViewService(this.Settings, this.postService);
		}
       

		[HttpGet]
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			
			var posts = await postService.FetchPosts();

			posts = posts.Where(p => !p.Reviewed)
						 .OrderByDescending(p => p.Date).ThenByDescending(p => p.LastUpdated);

			bool withCategories = true;
			var pageList = await viewService.GetPostPagedList(posts, page, pageSize, withCategories);


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody]  PostReviewForm model)
		{
			if(model.postIds.IsNullOrEmpty()) return BadRequest();

			await postService.ReviewPosts(model.postIds);
			foreach (var id in model.postIds)
			{
				var post =await postService.GetByIdAsync(id);
				post.Reviewed = true;
			}

			return new NoContentResult();


		}
	}
}