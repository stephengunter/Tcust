using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using Tcust.Services;
using Tcust.Models;
using Tcust.Views;
using Tcust.Helpers;

namespace WebApi.Tcust
{
	

	public class TermsController : BaseApiController
	{
		private readonly ITermService termService;

		public TermsController(ITermService termService)
		{
			this.termService = termService;
		}

		public async Task<IEnumerable<TermYear>> Index()
		{
			bool withTerms = true;
			var termYears = await termService.GetAllTermYearsAsync(withTerms);

			termYears = termYears.GetOrdered();

			return termYears;

		}


		public IActionResult GetActiveTerm()
		{
			var term = termService.GetActiveTerm();

			var model = term.MapTermViewModel();


			return Ok(model);


		}


	}

}