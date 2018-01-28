using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HistoryApp.Areas.Expo.Controllers
{
    public class PostsController : BaseExpoController
	{
		public IActionResult test()
		{
			return Content("");
		}

		public IActionResult Index()
        {
            return View();
        }
		public IActionResult Diaries()
		{
			return View();
		}
		public IActionResult HonorTop()
		{
			return View();
		}
		public IActionResult Honors()
		{
			return View();
		}
		public IActionResult HonorSearch()
		{
			return View();
		}
		
		public IActionResult News()
		{
			return View();
		}
	}
}