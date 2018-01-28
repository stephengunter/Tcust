using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Permissions.Services;

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

namespace BlogWeb.Areas.Admin.Controllers
{
	[Authorize(Policy = "EDIT_POSTS")]
	public class ClicksController : BaseAdminController
	{
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public ClicksController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService, IPostService postService) 
			: base(environment, settings, permissionService)

		{
			this.postService = postService;

			this.viewService = new ViewService(this.Settings, this.postService);
		}
		public async Task<IActionResult> Index(string period = "month", string sort = "desc", int page = 1, int pageSize = 10)
        {
			List<PostClickModel> groupResult = null;

			bool desc = sort != "asc";

			if (period == "month")
			{
				var begin = DateTime.Today.AddMonths(-1);
				var end = DateTime.Today;

				groupResult = postService.GroupPostByClicksInPeriod(begin, end, desc);
			}
			else
			{
				groupResult = postService.GroupPostByClicks(desc);
			}
			
			
			var pageList = new PagedList<PostClickModel, PostViewModel>(groupResult, page, pageSize);

			var postIds = pageList.List.Select(p=>p.postId).ToList();
			var posts = postService.ListByIds(postIds);

			foreach (var item in pageList.List)
			{
				var post = posts.Where(p => p.Id == item.postId).FirstOrDefault();

				var postViewModel = viewService.MapPostViewModel(post);
				postViewModel.clickCount = item.clickCount;

				var categories = await postService.GetPostCategoriesAsync(post);
				postViewModel.categoryName = String.Join(",", categories.Select(c => c.Name));

				pageList.ViewList.Add(postViewModel);
			}

			pageList.List = null;

			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			var periodOptions = GetPeriodOptions();

			ViewData["periods"] = this.ToJsonString(periodOptions);

			ViewData["list"] = this.ToJsonString(pageList);

			return View();

		}

		private List<BaseOption> GetPeriodOptions()
		{
			return new List<BaseOption>
			{
				new BaseOption("month","最近一個月"),
				new BaseOption("all","有史以來")
			};
		}
    }
}