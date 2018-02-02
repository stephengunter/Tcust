using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
	
	[Authorize]
	public class IdentityController : BaseApiController
	{
		[HttpGet]
		public IActionResult Index()
		{
			return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
		}
	}
}