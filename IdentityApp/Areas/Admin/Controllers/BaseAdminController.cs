using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Controllers;

namespace IdentityApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles ="Dev")]
	public class BaseAdminController : BaseController
	{
         
    }
}