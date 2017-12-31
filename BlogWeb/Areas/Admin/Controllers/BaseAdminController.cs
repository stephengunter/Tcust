using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public abstract class BaseAdminController: BlogWeb.Controllers.BaseController
	{
    }
}
