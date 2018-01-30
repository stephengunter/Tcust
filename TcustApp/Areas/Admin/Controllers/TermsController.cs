using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Tcust.Services;
using Tcust.Helpers;
using System.Linq;
using ApplicationCore.Helpers;
using Tcust.Models;
using Tcust.Views;
using ApplicationCore.Views;
using System.Collections.Generic;

namespace TcustApp.Areas.Admin.Controllers
{
	public class TermsController : BaseAdminController
	{
		private readonly ITermService termService;

		public TermsController(IHostingEnvironment environment, IOptions<AppSettings> settings, ITermService termService)
			  : base(environment, settings)
		{

			this.termService = termService;
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var terms = await termService.GetAllTermAsync();

			terms = terms.GetOrdered();

			var pageList = terms.ToPageList(page, pageSize);

			if (Request.IsAjaxRequest())
			{
				return Ok(pageList);
			}

			ViewData["list"] = this.ToJsonString(pageList);

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var yearOptions = await this.GetTermYearOptions();

			var termModel = new TermViewModel
			{
				termYearId= Convert.ToInt16(yearOptions.FirstOrDefault().value)
			};


			var model = new TermEditForm { term = termModel, yearOptions= yearOptions , order = 1 };

			return Ok(model);
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] TermEditForm model)
		{
			var termYear =await termService.GetTermYearByIdAsync(model.term.termYearId);
			if (termYear == null) throw new Exception(String.Format("TermYear not found.id = {0}", model.term.termYearId));

			int number = ViewService.CreateTermNumber(termYear.Year, model.order);

			var exist = termService.GetTermByNumber(number);
			if (exist != null)
			{
				ModelState.AddModelError("order", "年度與順序重複了");
				return BadRequest(ModelState);
			}
			

			var term = model.term.MapToEntity(number);


			term = await termService.CreateAsync(term);


			return Ok(term);


		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
			var term = await termService.GetByIdAsync(id);
			if (term == null) return NotFound();

			var yearOptions = await this.GetTermYearOptions();

			var model = new TermEditForm
			{
				term = term.MapTermViewModel(),
				yearOptions = yearOptions,
				order= term.Order
			};


			return Ok(model);
		}

		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] TermEditForm model)
		{
			var term = await termService.GetByIdAsync(id);
			if (term == null) return NotFound();

			var termYear = await termService.GetTermYearByIdAsync(model.term.termYearId);
			if (termYear == null) throw new Exception(String.Format("TermYear not found.id = {0}", model.term.termYearId));

			
			int number = ViewService.CreateTermNumber(termYear.Year, model.order);

			var exist = termService.GetTermByNumber(number);
			if (exist != null && exist.Id!=id)
			{
				ModelState.AddModelError("order", "年度與順序重複了");
				return BadRequest(ModelState);
			}


			term = model.term.MapToEntity(number,term);

			await termService.UpdateAsync(term);


			return Ok(term);


		}


		private async Task<List<BaseOption>> GetTermYearOptions()
		{
			
			var termYears = await termService.GetAllTermYearsAsync();
			termYears = termYears.GetOrdered();


			return termYears.ToOptions();

		}
	}
}