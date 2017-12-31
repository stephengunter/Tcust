using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog.Services;
using Blog.Models;
using ApplicationCore.Paging;
using ApplicationCore.Helpers;

namespace WebApi.Controllers.Blog
{
    [Produces("application/json")]
	[Route("api/blog/[controller]")]
	public class TopPostsController : Controller
    {
		private readonly ITopPostService topPostService;

		public TopPostsController(ITopPostService topPostService)
		{
			this.topPostService = topPostService;
		}


		//[HttpGet]
		//public async Task<IPagedList<Post>> Get(PagingParams pagingParams)
		//{
		//	var posts = await topPostService.GetTopPosts(pagingParams);

		//	//var outputModel = new PostOutputModel
		//	//{
		//	//	Paging = model.GetHeader(),
		//	//	Links = GetLinks(model),
		//	//	Items = model.List.Select(m => ToMovieInfo(m)).ToList(),
		//	//};
		//	return posts;
		//}

		

		[HttpGet]
		public IActionResult Get()
		{
			var posts = topPostService.GetTopPosts();

			return Ok(posts);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var post = topPostService.GetById(id);
			if (post == null)
			{
				return NotFound();
			}
			return new ObjectResult(post);
		}

		//[HttpGet]
		//public async Task<IActionResult> Get()
		//{
		//	return Content("test");
		//	var posts = await topPostService.GetTopPosts();

		//	var pagedList = posts.AsQueryable().ToPageList(1,10);

		//	//Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

		//	//var outputModel = new PostOutputModel
		//	//{
		//	//	Paging = model.GetHeader(),
		//	//	Items = model,
		//	//};
		//	return Ok(pagedList);
		//}


		//[HttpGet(Name = "GetMovies")]
		//public IActionResult Get(PagingParams pagingParams)
		//{
		//	var model = service.GetMovies(pagingParams);

		//	Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

		//	var outputModel = new MovieOutputModel
		//	{
		//		Paging = model.GetHeader(),
		//		Links = GetLinks(model),
		//		Items = model.List.Select(m => ToMovieInfo(m)).ToList(),
		//	};
		//	return Ok(outputModel);
		//}

		//private List<LinkInfo> GetLinks(PagedList<Movie> list)
		//{
		//	var links = new List<LinkInfo>();

		//	if (list.HasPreviousPage)
		//		links.Add(CreateLink("GetMovies", list.PreviousPageNumber,
		//				   list.PageSize, "previousPage", "GET"));

		//	links.Add(CreateLink("GetMovies", list.PageNumber,
		//				   list.PageSize, "self", "GET"));

		//	if (list.HasNextPage)
		//		links.Add(CreateLink("GetMovies", list.NextPageNumber,
		//				   list.PageSize, "nextPage", "GET"));

		//	return links;
		//}

	}

	public class PostOutputModel
	{
		public IPagingHeader Paging { get; set; }
		public List<LinkInfo> Links { get; set; }
		public List<Post> Items { get; set; }
	}

	public class MovieInfo
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int ReleaseYear { get; set; }
		public string Summary { get; set; }
		public DateTime LastReadAt { get; set; }
	}
}