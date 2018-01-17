﻿using System;
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
	[Authorize(Policy = "EDIT_POSTS")]
	public class PostsController: BaseAdminController
	{
		private readonly IPostService postService;
		

		private readonly ViewService viewService;

		public PostsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService ,
			IPostService postService) : base(environment, settings, permissionService)
		{
			
			this.postService = postService;
			

			this.viewService = new ViewService(this.Settings); 
		}

		

		
		[HttpGet]
		public async Task<IActionResult> Index(int category=0 ,string keyword="" ,int page = 1, int pageSize=10 )
		{
			Category selectedCategory = null;
			if (category > 0) selectedCategory = await postService.GetCategoryByIdAsync(category);
			if (selectedCategory == null) category = 0;

			var posts = await postService.FetchPosts(selectedCategory, keyword);

			posts = posts.Where(p=>p.Reviewed)
					     .OrderByDescending(p=>p.Date).ThenByDescending(p=>p.LastUpdated);

			var pageList = new PagedList<Post, PostViewModel>(posts, page, pageSize);

			foreach (var item in pageList.List)
			{
				var postViewModel = viewService.MapPostViewModel(item);
				postViewModel.clickCount = await postService.GetPostClickCount(item.Id);

				pageList.ViewList.Add(postViewModel);
			}

			pageList.List = null;
			

			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}

			bool edit = false;
			var categoryOptions = await GetCategoryOptions(edit);

			ViewData["categories"] = this.ToJsonString(categoryOptions);
			

			ViewData["list"]= this.ToJsonString(pageList);

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			bool canReviewPost = CanReviewPost();
			var post = new PostViewModel()
			{
				termNumber = DefaultTermNumber(),
				reviewed= canReviewPost
			};

			bool edit = true;
			var categoryOptions = await GetCategoryOptions(edit);

			var model = new PostEditForm
			{
				post = post,
				categoryOptions= categoryOptions,
				canReview= canReviewPost.ToInt()

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

			bool canReviewPost = CanReviewPost();

			bool allMedias = true;
			var model = new PostEditForm
			{
				post = viewService.MapPostViewModel(post, allMedias),
				canReview = canReviewPost.ToInt()
			};

			var categoryIds =await postService.GetCategoryIdsAsync(id);

			model.post.categoryId = categoryIds.FirstOrDefault();

			bool edit = true;
			var categoryOptions = await GetCategoryOptions(edit);

			model.categoryOptions = categoryOptions;

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


		private async Task<List<BaseOption>> GetCategoryOptions(bool edit)
		{
			bool emptyOption = !edit;
			var categories = await postService.GetCategoriesAsync();

			return categories.ToOptions(emptyOption);

		}



	}
}