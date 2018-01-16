using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Areas.Api.Controllers
{
	[Area("Api")]
	[Authorize]
	public abstract class BaseApiController : Controller
    {
       
    }
}