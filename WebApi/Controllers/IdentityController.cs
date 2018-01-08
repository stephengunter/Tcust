using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
	//public class Product
	//{
	//	public string name { get; set; }
	//}

    [Route("api/[controller]")]
	[Authorize]
	public class IdentityController : Controller
    {
		[HttpGet]
		public IActionResult Get()
		{
			//var products = new List<Product> {
			//	new Product{  name="test"}
			//};
			//return new JsonResult(products);
			return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
		}
    }
}