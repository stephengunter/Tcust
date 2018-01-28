
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApplicationCore.Controllers
{
	public abstract class BaseController : Controller
	{
		private readonly JsonSerializerSettings jsonSettings;

		public BaseController()
		{
			this.jsonSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			};
		}

		protected string ToJsonString(object model)
		{
			return JsonConvert.SerializeObject(model, this.jsonSettings);
		}

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

		protected async Task<string> GetToken()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			return accessToken;
		}

	}
}