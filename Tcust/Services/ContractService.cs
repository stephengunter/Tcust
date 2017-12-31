using System;
using System.Collections.Generic;
using System.Text;
using Tcust.DAL;
using Tcust.Models;

namespace Tcust.Services
{
	public interface IContractService
	{

	}

	public class ContractService
    {
		private readonly ITcustRepository<Contract> contractRepository;
		private readonly ITcustRepository<Tcust.Models.Type> typeRepository;
		private readonly ITcustRepository<Department> departmentRepository;

		public ContractService(ITcustRepository<Contract> contractRepository, ITcustRepository<Tcust.Models.Type> typeRepository, ITcustRepository<Department> departmentRepository)
		{
			this.contractRepository = contractRepository;
			this.typeRepository = typeRepository;
			this.departmentRepository = departmentRepository;
		}

		
		//void Create(Contract contract , ICollection<Partner> partners, ICollection<Department> Departments, ICollection<Tcust.Models.Type> types)
		//{
		//	foreach (var partner in partners)
		//	{
		//		contract.Partners.Add(partner);
		//	}
			

		//	//if (enterprise) partner.Type = PartnerType.Enterprise;
		//	//else partner.Type = PartnerType.School;


		//	//foreach (var typeId in typeIds)
		//	//{
		//	//	var type = typeRepository.GetById(typeId);
		//	//	contract.Types.Add(type);

		//	//}

		//}

		public void Create(Contract contract, Partner partner, bool enterprise, int[] typeIds, int[] departmentIds,string updatedBy)
		{
			if (enterprise) partner.Type = PartnerType.Enterprise;
			else partner.Type = PartnerType.School;

			partner.SetBaseRecord(updatedBy);
			contract.SetBaseRecord(updatedBy);

			contract.Partners.Add(partner);

			foreach (var typeId in typeIds)
			{
				var type = typeRepository.GetById(typeId);
				contract.Types.Add(type);

			}

			foreach (var departmentId in departmentIds)
			{
				var department = departmentRepository.GetById(departmentId);
				contract.Departments.Add(department);

			}

			contractRepository.Add(contract);
		}
	}
}
