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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Blog.Helpers;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Views;
using Permissions.Services;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Authorize(Policy = "REVIEW_POSTS")]
	public class TopsController : BaseAdminController
	{
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public TopsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService, IPostService postService) 
			: base(environment, settings, permissionService)
		{

			this.postService = postService;

			this.viewService = new ViewService(this.Settings, this.postService);
		}


		[HttpGet]
		public async Task<IActionResult> Index(int category = 0,  int page = 1, int pageSize = 10)
		{
			bool returnDefault = true;
			Category selectedCategory = await postService.GetCategoryByIdAsync(category, returnDefault);
			if (selectedCategory == null) category = 0;

			var posts = await postService.FetchPosts(selectedCategory);

			posts = posts.Where(p => p.Top);

			posts = viewService.OrderPosts(posts);


			bool withCategories = true;
			var pageList = await viewService.GetPostPagedList(posts, page, pageSize, withCategories);


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			bool edit = true;
			var categoryOptions = await GetCategoryOptions(edit);

			ViewData["categories"] = this.ToJsonString(categoryOptions);


			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] IList<TopPostEditForm> models)
		{
			foreach (var model in models)
			{
				var post = await postService.GetByIdAsync(model.id);

				if (model.order >= 0)
				{
					post.DisplayOrder = model.order;
				}
				else
				{
					post.Top = false;
				}

				await postService.UpdateAsync(post);


			}

			return new NoContentResult();



		}

		private async Task<List<BaseOption>> GetCategoryOptions(bool edit)
		{
			bool emptyOption = !edit;
			var categories = await postService.GetCategoriesAsync();

			return categories.ToOptions(emptyOption);

		}


	}
}