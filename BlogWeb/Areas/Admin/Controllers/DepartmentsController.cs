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

using Tcust.Services;
using Tcust.Models;


namespace BlogWeb.Areas.Admin.Controllers
{
    public class DepartmentsController : BaseAdminController
	{
		private readonly IDepartmentService departmentService;
		private readonly IPostService postService;
		private readonly ITargetService targetService;
		private readonly ITermService termService;

		public DepartmentsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService,
			IDepartmentService departmentService, IPostService postService, ITargetService targetService, ITermService termService) : base(environment, settings, permissionService)
		{
			this.postService = postService;
			this.departmentService = departmentService;
			this.targetService = targetService;
			this.termService = termService;
		}


		Department defaultDepartment;
		Department GetDefaultDepartment()
		{
			if (this.defaultDepartment != null) return this.defaultDepartment;
			
			this.defaultDepartment= departmentService.GetByCode("103010");
			return this.defaultDepartment;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string term, int type=0)
        {
			int termNumber = 0;

			if (!Request.IsAjaxRequest()) type = 0;

			var terms = await termService.GetAllTermAsync();
			terms = terms.OrderByDescending(t => t.Active).OrderByDescending(t => t.Number);

			var departments = await departmentService.GetAllAsync();

			var defaultDepartment = GetDefaultDepartment();

			if (String.IsNullOrEmpty(term))
			{
				termNumber = terms.FirstOrDefault().Number;
			}
			else
			{
				var exist = terms.Where(t => t.Number == term.ToInt()).FirstOrDefault();
				if (exist == null) termNumber = terms.FirstOrDefault().Number;

				else termNumber = exist.Number;

			}

			var posts = await postService.GetAllAsync();
			posts = posts.Where(p => p.TermNumber == termNumber);

			if (type == 1)
			{
				return await ByMonth(posts,departments);
			}

			var targets = await targetService.FetchByTerm(termNumber);
			if (targets.IsNullOrEmpty())
			{
				await InitDepartmentTargets(termNumber, departments);
			}


			var pageList = GetTargetPagedList(targets);
			foreach (var targetView in pageList.ViewList)
			{
				var department = departments.Where(d => d.Id == targetView.departmentId).FirstOrDefault();
				if (department == null) continue;
				targetView.departmentName = department.Name;
				var postIds = await  postService.GetPostsIdsByDepartmentAsync(department);

				var postsByDepartment = posts.Where(p=> postIds.Contains(p.Id));

				targetView.total = 0;
				targetView.sub = 0;

				foreach (var post in postsByDepartment)
				{
					targetView.total += 1;

					var issuers = await postService.GetIssuersByPostAsync(post);

					var issueByDefaultDepartment = await IssueByDefaultDepartment(post, defaultDepartment);


					if (issueByDefaultDepartment) targetView.sub += 1;

				}
			}

			

			if (Request.IsAjaxRequest())
			{
				var viewModel = new TargetIndexViewModel();
				viewModel.TargetViewList = pageList;
				return new ObjectResult(viewModel);
			}

			var termOptions = terms.Select(t => new BaseOption(t.Number.ToString(), t.Number.ToString())).ToList();
			ViewData["terms"] = this.ToJsonString(termOptions);

			ViewData["list"] = this.ToJsonString(pageList);

			ViewData["can_delete"] = CanReviewPost().ToInt();

			return View();
		}

		public async Task<IActionResult> ByMonth(IEnumerable<Post> posts , IEnumerable<Department> departments)
		{
			var monthes = posts.Select(p => p.Month).Distinct();
			var model = new TargetIndexViewModel();
			model.Monthes = monthes.ToList();
			model.Monthes.Sort();

			var pageList = new PagedList<Department, TargetViewModel>(departments);

			foreach (var department in departments)
			{
				var postIds = await postService.GetPostsIdsByDepartmentAsync(department);

				var viewModel = new TargetViewModel()
				{
					departmentName = department.Name,
					monthPostCounts=new List<int>()
				};

				foreach (int month in model.Monthes)
				{
					var postsInMonth = posts.Where(p => p.Month == month);

					var postsByDepartment = postsInMonth.Where(p => postIds.Contains(p.Id));

					viewModel.monthPostCounts.Add(postsByDepartment.Count());
				}
				

				pageList.ViewList.Add(viewModel);
				

				
			}

			model.MonthlyViewList = pageList;

			pageList.List = null;
			return new ObjectResult(model);
		}

		//saveTarget
		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] IList<TargetEditForm> models)
		{
			foreach (var model in models)
			{
				var target = await targetService.GetByIdAsync(model.id);

				target.Tatget = model.target;

				await targetService.UpdateAsync(target);


			}

			return new NoContentResult();

		}

		public PagedList<DepartmentTarget, TargetViewModel> GetTargetPagedList(IEnumerable<DepartmentTarget> targets, int page=1, int pageSize=999)
		{
			var pageList = new PagedList<DepartmentTarget, TargetViewModel>(targets, page, pageSize);

			foreach (var departmentTarget in pageList.List)
			{
				if (departmentTarget.DepartmentId == this.GetDefaultDepartment().Id) continue;
				var viewModel = new TargetViewModel()
				{
					id= departmentTarget.Id,
					target = departmentTarget.Tatget,
					departmentId= departmentTarget.DepartmentId,

					newTarget= departmentTarget.Tatget,
				};

				pageList.ViewList.Add(viewModel);
			}

			pageList.List = null;

			return pageList;
		}

		async Task InitDepartmentTargets(int termNumber, IEnumerable<Department> departments)
		{
			foreach (var department in departments)
			{
				var exist= targetService.GetByDepartment(department, termNumber);
				if (exist == null)
				{
					await targetService.CreateAsync(new DepartmentTarget { DepartmentId = department.Id, TermNumber = termNumber, Tatget = 0 });
				}

				
			}
		}

		async Task<bool> IssueByDefaultDepartment(Post post,Department defaultDepartment)
		{
			var issuers = await postService.GetIssuersByPostAsync(post);
			if (issuers.IsNullOrEmpty()) return false;
			
			if (issuers.Count() > 1) return false;


			return issuers.FirstOrDefault().DepartmentId == defaultDepartment.Id;
		}
		

	}
}