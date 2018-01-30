using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Tcust.Services;
using Tcust.Helpers;
using ApplicationCore.Helpers;
using Tcust.Models;
using Tcust.Views;

namespace TcustApp.Areas.Admin.Controllers
{
    public class YearsController : BaseAdminController
	{
		private readonly ITermService termService;

		public YearsController(IHostingEnvironment environment, IOptions<AppSettings> settings, ITermService termService)
		      :base(environment, settings)
		{

			this.termService = termService;
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
			var termYears =await termService.GetAllTermYearsAsync();

			termYears = termYears.GetOrdered();

			var pageList = termYears.ToPageList(page, pageSize);

			if (Request.IsAjaxRequest())
			{
				return Ok(pageList);
			}

			ViewData["list"] = this.ToJsonString(pageList);

			return View();
        }

		[HttpGet]
		public IActionResult Create()
		{
			var activeTermYear = termService.GetActiveTermYear();

			var termYear = new TermYearViewModel { year = activeTermYear.Year + 1 };
			var model = new TermYearEditForm { year = termYear };

			return Ok(model);
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] TermYearEditForm model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var exist = termService.GetTermYear(model.year.year);
			if (exist != null)
			{
				ModelState.AddModelError("year.year", "年度重複了");
				return BadRequest(ModelState);
			}

			var termYear = model.year.MapToEntity();

			if (termYear.Title.IsNullOrEmpty())
			{
				termYear.Title = String.Format("{0}學年度", termYear.Year);
			}


			termYear= await termService.CreateTermYearAsync(termYear);


			return Ok(termYear);


		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
			var termYear = await termService.GetTermYearByIdAsync(id);
			if (termYear == null) return NotFound();

			var model = new TermYearEditForm
			{
				year = termYear.MapTermYearViewModel()
			};


			return Ok(model);
		}

		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] TermYearEditForm model)
		{
			var termYear = await termService.GetTermYearByIdAsync(id);
			if (termYear == null) return NotFound();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var exist = termService.GetTermYear(model.year.year);
			if (exist != null && exist.Id!=id)
			{
				ModelState.AddModelError("year.year", "年度重複了");
				return BadRequest(ModelState);
			}

			termYear = model.year.MapToEntity(termYear);

			await termService.UpdateTermYearAsync(termYear);


			return Ok(termYear);


		}
	}
}