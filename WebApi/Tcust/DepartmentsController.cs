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
    public class DepartmentsController : BaseApiController
	{
		private readonly IDepartmentService departmentService;

		public DepartmentsController(IDepartmentService departmentService)
		{
			this.departmentService = departmentService;
		}

		public async Task<IEnumerable<Department>> Index()
		{

			return await departmentService.GetAllAsync();
		}
    }
}