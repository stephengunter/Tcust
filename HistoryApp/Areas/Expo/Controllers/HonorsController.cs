using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HistoryApp.Areas.Expo.Controllers
{
	public class HonorsController : BaseExpoController
	{
		public IActionResult Index()
		{
			return View("Top");
		}

		public IActionResult List()
		{
			return View();
		}

		public IActionResult Search()
		{
			return View();

		}

	}
}