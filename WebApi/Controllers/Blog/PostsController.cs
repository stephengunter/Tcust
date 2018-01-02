using System;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blog.DAL;
using System.Linq;

namespace WebApi.Controllers.Blog
{
	[Produces("application/json")]
	[Route("api/blog/[controller]")]
	public class PostsController : Controller
	{
		private readonly IPostService postService;

		public PostsController(IPostService postService)
		{
			this.postService = postService;
		}


		//public PostsController(IBlogRepository<Post> postRepository, IBlogRepository<UploadFile> uploadFileRepository)
		//{
		//	this.postRepository = postRepository;
		//	this.uploadFileRepository = uploadFileRepository;
		//}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var post = await postService.GetByIdAsync(id);
			

			if (post == null) return NotFound();

			return new ObjectResult(post);
		}

		Post addPost()
		{
			var post = new Post
			{
				CreatedAt = DateTime.Now,
				Top = false,
				LastUpdated = DateTime.Now,
			};
			post.Attachments = new List<UploadFile>();
			post.Attachments.Add(new UploadFile()
			{
				CreatedAt = DateTime.Now,
			
				LastUpdated = DateTime.Now,

			});

			post.Attachments.Add(new UploadFile()
			{
				CreatedAt = DateTime.Now,
				
				LastUpdated = DateTime.Now,

			});

			return postService.Create(post);


			//postRepository.Add(post);
		}

		void updatePost(Post post)
		{
			post.Title = "new Title";

			post.Attachments = new List<UploadFile>();

			post.Attachments.Add(new UploadFile()
			{
				CreatedAt = DateTime.Now,
			
				LastUpdated = DateTime.Now,

			});

			post.Attachments.Add(new UploadFile()
			{
				CreatedAt = DateTime.Now,
				
				LastUpdated = DateTime.Now,

			});

			postService.Update(post);
		}

		void deletePost(Post post)
		{
			postService.Delete(post);
		}

		//[HttpGet("{id}")]
		//public IActionResult GetById(int id)
		//{
			
		//	//var post = postService.GetById(id);
		//	//deletePost(post);
				

		//	return new ObjectResult(null);



		//}


		//[HttpGet("search/{keyword}")]
		//public async Task<IEnumerable<Post>> Search(string keyword)
		//{
		//	var posts = await postService.GetByKeywordAsync(keyword);



		//	return posts;
		//}

		//[HttpGet]
		//public IEnumerable<Post> GetAll()
		//{
		//	var all = postRepository.DbSet.ToList();
		//	return all;
		//}
	}
}