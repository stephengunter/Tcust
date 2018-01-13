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
		private readonly IPostService postService;

		private readonly ViewService viewService;

		public PostsController(IHostingEnvironment environment, IOptions<AppSettings> settings,IPostService postService) : base(environment, settings)
		{
			
			this.postService = postService;

			this.viewService = new ViewService(this.Settings); 
		}

		public IActionResult Test(int postId, int categoryId)
		{
			var user = User.Identity.Name;
			//bool returnDefaultCategory = true;
			//var selectedCategory = await postService.GetCategoryByIdAsync(0, returnDefaultCategory);

			//var posts =await postService.GetAllAsync();
			//posts = posts.Take(60);

			//foreach (var post in posts)
			//{
			//	post.Categories.Add(selectedCategory);
			//	postService.Update(post);
			//}

			return Content(user);
			
		}

		
		[HttpGet]
		public async Task<IActionResult> Index(int category=0 ,string keyword="" ,int page = 1, int pageSize=10 )
		{
			Category selectedCategory = null;
			if (category > 0) selectedCategory = await postService.GetCategoryByIdAsync(category);
			if (selectedCategory == null) category = 0;

			var posts = await postService.FetchPosts(selectedCategory, keyword);

			posts = posts.OrderByDescending(p=>p.Date).ThenByDescending(p=>p.LastUpdated);

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

			var categories =await postService.GetCategoriesAsync();
			var options = categories.Select(c => new { value = c.Id, text = c.Name });

			ViewData["categories"] = this.ToJsonString(options);
			

			ViewData["list"]= this.ToJsonString(pageList);

			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			var post = new PostViewModel()
			{
				termNumber = DefaultTermNumber()
			};
			var model = new PostEditForm
			{
				post = post
			};
		


			return new ObjectResult(model);
		}

		
		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] PostEditForm model)
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

			var caregory = await postService.GetCategoryByIdAsync(model.post.categoryId);
			post.Categories.Add(caregory);



			

			post = await postService.CreateAsync(post);


			return new ObjectResult(post);

		
		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
			var post = postService.GetById(id);
			if (post == null) return NotFound();

			bool allMedias = true;
			var model = new PostEditForm
			{
				post = viewService.MapPostViewModel(post, allMedias)
			};

			var categoryIds =await postService.GetCategoryIdsAsync(id);

			model.post.categoryId = categoryIds.FirstOrDefault();

			return new ObjectResult(model);
		}

		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id , [FromBody] PostEditForm model)
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

			var categoryIds =new List<int> { model.post.categoryId };
			
			


			await postService.UpdateAsync(post, categoryIds);


			return new ObjectResult(post);


		}

		


		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			await postService.DeleteAsync(id, CurrentUserId);

			return new NoContentResult();
			
		}



	}
}