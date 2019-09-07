using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tcust.DAL;
using Tcust.Models;
using Tcust.Specifications;
using System.Linq;

namespace Tcust.Services
{
	public interface IDepartmentService
	{
		Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> CreateAsync(Department department);
        Task UpdateAsync(Department department);
        Task<Department> GetByIdAsync(int id);
        Department GetByCode(string code);
        Task<IEnumerable<Department>> FetchByCodesAsync(IEnumerable<string> codes);

    }


	public class DepartmentService: IDepartmentService
	{
		private readonly ITcustRepository<Department> departmentRepository;

		public DepartmentService(ITcustRepository<Department> departmentRepository)
		{
			this.departmentRepository = departmentRepository;
		}

		public async Task<IEnumerable<Department>> GetAllAsync()
		{

			return await departmentRepository.ListAllAsync();
		}

		public async Task<Department> GetByIdAsync(int id) => await departmentRepository.GetByIdAsync(id);

        public async Task<Department> CreateAsync(Department department) => await departmentRepository.AddAsync(department);

        public async Task UpdateAsync(Department department) => await departmentRepository.UpdateAsync(department);

        public Department GetByCode(string code)
		{
			var filter = new DepartmentFilterSpecifications(code);
			return departmentRepository.GetSingleBySpec(filter);
		}

        public async Task<IEnumerable<Department>> FetchByCodesAsync(IEnumerable<string> codes)
        {
            var filter = new DepartmentFilterSpecifications(codes);
            return await departmentRepository.ListAsync(filter);
        }



    }
}
