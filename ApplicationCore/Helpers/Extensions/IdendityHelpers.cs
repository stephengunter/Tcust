using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Helpers
{


	public static class IdendityHelpers
	{
		public static string CurrentUserId(this AuthorizationHandlerContext context)
		{
			var entity= context.User.Claims.Where(c => c.Type == "sub").FirstOrDefault();
			if (entity == null) return "";

			return entity.Value;
			

		}

		public static bool CurrentUserIsDev(this AuthorizationHandlerContext context)
		{
			return context.User.IsInRole("Dev");
		}
		
	}
}
