
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;
using System;
using ApplicationCore.Views;
using Tcust.Models;

namespace Blog.Services
{
	public interface ITargetService
	{
		Task<IEnumerable<DepartmentTarget>> FetchByTerm(int termNumber);
		Task<DepartmentTarget> GetByIdAsync(int id);
		Task<DepartmentTarget> CreateAsync(DepartmentTarget target);
		Task UpdateAsync(DepartmentTarget target);

		DepartmentTarget GetByDepartment(Department department, int termNumber);
	}

	public class TargetService: ITargetService
	{
		private readonly IBlogRepository<DepartmentTarget> departmentTargetRepository;


		public TargetService(IBlogRepository<DepartmentTarget> departmentTargetRepository)
		{
			this.departmentTargetRepository = departmentTargetRepository;
		}

		public async Task<IEnumerable<DepartmentTarget>> FetchByTerm(int termNumber)
		{
			var filter = new TargetFilterSpecification(termNumber);
			return await departmentTargetRepository.ListAsync(filter);


		}

		public DepartmentTarget GetByDepartment(Department department, int termNumber)
		{
			var filter = new TargetFilterSpecification(department, termNumber);
			return  departmentTargetRepository.GetSingleBySpec(filter);
		}

		public async Task<DepartmentTarget> GetByIdAsync(int id) => await departmentTargetRepository.GetByIdAsync(id);

		public async Task UpdateAsync(DepartmentTarget target) => await departmentTargetRepository.UpdateAsync(target);

		public async Task<DepartmentTarget> CreateAsync(DepartmentTarget target) => await departmentTargetRepository.AddAsync(target);
	}
}
