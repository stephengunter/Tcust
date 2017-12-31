using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Tcust.ApplicationCore.Entities;
using Tcust.ApplicationCore.Interfaces;

namespace WebApi.Controllers
{
    

	[Route("api/[controller]")]
	public class PostsController : BaseController
    {
		IAsyncRepository<Post> _postRepository;
		public PostsController(IAsyncRepository<Post> postRepository):base()
		{
			this._postRepository = postRepository;
		}

		[HttpGet]
		public async Task<IEnumerable<Post>> GetAll()
		{
			var posts= await this._postRepository.ListAllAsync();
			return posts;
		}

		[HttpGet("{id}", Name = "GetPost")]
		public async Task<IActionResult> GetById(long id)
		{
			var item = await this._postRepository.GetByIdAsync((int)id);

			if (item == null) return NotFound();


			return new ObjectResult(item);
		}


	}
}