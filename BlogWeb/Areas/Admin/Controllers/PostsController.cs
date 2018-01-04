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

using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using BlogWeb.Helpers;

namespace BlogWeb.Areas.Admin.Controllers
{

	public class PostsController: BaseAdminController
	{
		private readonly IOptions<AppSettings> settings;

		
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public PostsController(IHostingEnvironment environment, IOptions<AppSettings> settings,IPostService postService) : base(environment)
		{
			this.settings = settings;
			this.postService = postService;

			this.viewService = new ViewService(this.settings); 
		}

		public IActionResult Test()
		{

			
			return View();
		}

		
		[HttpGet]
		public async Task<IActionResult> Index(string keyword ,int page = 1, int pageSize=10 )
		{
			Task<IEnumerable<Post>> getPostsTask;
			if (String.IsNullOrEmpty(keyword))
			{
				getPostsTask =  postService.GetAllAsync();
			}
			else
			{
				getPostsTask = postService.GetByKeywordAsync(keyword);
			}

			var posts = await getPostsTask;

			posts = posts.OrderByDescending(p=>p.Date);

			var pageList = new PagedList<Post, PostViewModel>(posts, page, pageSize);

			foreach (var item in pageList.List)
			{
				pageList.ViewList.Add(viewService.MapPostViewModel(item));
			}

			pageList.List = null;


			//var model = new PostSearchModel();
			//model.PagedList = pageList;

			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			ViewBag.list = this.ToJsonString(pageList);

			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			var model = new PostEditForm
			{
				post = new PostViewModel()
			};
		


			return new ObjectResult(model);
		}

		
		[HttpPost("[area]/[controller]")]
		public IActionResult Store([FromBody] PostEditForm model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var post = model.post.MapToEntity(CurrentUserId);

			foreach (var  item in model.post.medias)
			{
				post.Attachments.Add(item.MapToEntity(CurrentUserId));
			}
			
		

			post = postService.Create(post);


			return new ObjectResult(post);

		
		}

		[HttpPut("[area]/[controller]/{id}")]
		public IActionResult Update(int id , [FromBody] PostEditForm model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var post = postService.GetById(id);
			if (post == null) return NotFound();

			post = model.post.MapToEntity(CurrentUserId, post);

			if (post.Attachments.IsNullOrEmpty()) post.Attachments = new List<UploadFile>();

			foreach (var item in model.post.medias)
			{
				var attachment = post.Attachments.Where(a=>a.Id== item.id).FirstOrDefault();

				if (attachment == null) post.Attachments.Add(item.MapToEntity(CurrentUserId));				
				else attachment = item.MapToEntity(CurrentUserId, attachment);
				
			}


			postService.Update(post);


			return new ObjectResult(post);


		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public IActionResult Edit(int id)
		{
			var post = postService.GetById(id);
			if (post == null) return NotFound();

			bool allMedias = true;
			var model = new PostEditForm
			{
				post = viewService.MapPostViewModel(post, allMedias)
			};

			return new ObjectResult(model);
		}


		[HttpDelete]
		public IActionResult Delete(int id)
		{
			postService.Delete(id, CurrentUserId);

			return new NoContentResult();
			
		}



	}
}