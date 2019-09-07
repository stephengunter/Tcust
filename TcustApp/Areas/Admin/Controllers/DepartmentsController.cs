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
	public class DepartmentsController : BaseAdminController
	{
		private readonly IDepartmentService departmentService;

		public DepartmentsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IDepartmentService departmentService)
			  : base(environment, settings)
		{

			this.departmentService = departmentService;
		}

		public async Task<IActionResult> Index()
		{
			var departments = await departmentService.GetAllAsync();

			departments = departments.GetOrdered();

			if (Request.IsAjaxRequest())
			{
				return Ok(departments);
			}

			ViewData["list"] = this.ToJsonString(departments);

			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			return Ok(new DepartmentViewModel());
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] DepartmentViewModel model)
		{
            ValidateRequest(model);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var department = model.MapToEntity();

			department = await departmentService.CreateAsync(department);

			return Ok(department);

		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public async Task<IActionResult> Edit(int id)
		{
			var department = await departmentService.GetByIdAsync(id);
			if (department == null) return NotFound();

            var model = department.MapDepartmentViewModel();

			return Ok(model);
		}

		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] DepartmentViewModel model)
		{
            var department = await departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();

            ValidateRequest(model);
            if (!ModelState.IsValid) return BadRequest(ModelState);

			department = model.MapToEntity(department);

			await departmentService.UpdateAsync(department);


			return Ok();


		}

        void ValidateRequest(DepartmentViewModel model)
        {
            if (!ModelState.IsValid) return;

            var exist = departmentService.GetByCode(model.code);
            if (exist != null && exist.Id != model.id)
            {
                ModelState.AddModelError("code", "代碼重複了");
            }
        }
    }
}