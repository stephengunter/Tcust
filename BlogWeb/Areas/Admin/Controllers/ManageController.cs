using Blog.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using ApplicationCore.Paging;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using Permissions.Services;
using Permissions.Models;
using Permissions.Views;

namespace BlogWeb.Areas.Admin.Controllers
{
	//[Authorize(Policy = "MANAGE_USERS")]
	public class ManageController : BaseAdminController
	{
		private readonly IPermissionService permissionService;

		public ManageController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService) : base(environment, settings)
		{
			this.permissionService = permissionService;
		}

		public async Task<IActionResult> Index(int permission = 0, string keyword = "", int page = 1, int pageSize = 10)
		{
			Permission selectedPermission = null;
			if (permission > 0) selectedPermission = await permissionService.GetPermissionByIdAsync(permission);
			if (selectedPermission == null) permission = 0;

			var users = await permissionService.FetchUsersWithPermissions(selectedPermission, keyword);

			users = users.OrderByDescending(u => u.LastUpdated);

			var pageList = new PagedList<AppUser, UserViewModel>(users, page, pageSize);

			foreach (var item in pageList.List)
			{
				pageList.ViewList.Add(PermissionViewService.MapUserViewModel(item));
			}

			pageList.List = null;


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			var permissions = await permissionService.GetPermissionsAsync();
			var options = permissions.Select(p => new { value = p.Id, text = p.Name });

			ViewData["permissions"] = this.ToJsonString(options);


			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{

			var model = new UserEditForm
			{
				user = new UserViewModel()
			};

			return new ObjectResult(model);
		}


		//[HttpPost("[area]/[controller]")]
		//public async Task<IActionResult> Store([FromBody] PostEditForm model)
		//{

		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	var post = model.post.MapToEntity(CurrentUserId);

		//	foreach (var item in model.post.medias)
		//	{
		//		post.Attachments.Add(item.MapToEntity(CurrentUserId));
		//	}

		//	var caregory = await postService.GetCategoryByIdAsync(model.post.categoryId);
		//	post.Categories.Add(caregory);





		//	post = await postService.CreateAsync(post);


		//	return new ObjectResult(post);


		//}
	}
}