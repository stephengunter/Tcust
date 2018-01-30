using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using Tcust.Services;
using Tcust.Models;

namespace WebApi.Tcust
{
	

	public class TermsController : BaseApiController
	{
		private readonly ITermService termService;

		public TermsController(ITermService termService)
		{
			this.termService = termService;
		}

		//public async Task<IEnumerable<TermYear>> Index()
		//{

		//	var termYears = await termService.GeTermYearsAsync();

		//	return termYears.OrderByDescending(t=>t.Active);

			

		//}


	}

}