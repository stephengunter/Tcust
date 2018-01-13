using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles ="Dev")]
	public class BaseAdminController : Controller
    {
         
    }
}