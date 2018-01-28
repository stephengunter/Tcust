using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
	[Produces("application/json")]
	public class BaseApiController : Controller
	{
		
		[HttpGet("api/[controller]/{id}")]
		public virtual async Task<IActionResult> Details(int id)
		{

			return NotFound();
		}

	}
}
