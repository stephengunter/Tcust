
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Controllers
{
	public abstract class BaseController : Controller
	{

		protected string CurrentUserId
		{
			get
			{

				var entity = User.Claims.Where(c => c.Type == "sub").FirstOrDefault();
				if (entity == null) return "";

				return entity.Value;

			}
		}

		protected bool CurrentUserIsDev
		{
			get
			{

				var entity = User.Claims.Where(c => c.Type == "role").FirstOrDefault();
				if (entity == null) return false;

				return entity.Value=="Dev";

			}
		}

	}
}