using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tcust.Models
{
	public class Contract : BaseRecord, IBaseContract
	{
		public Contract()
		{
			Departments = new JoinCollectionFacade<Department, Contract, DepartmentContract>(this, DepartmentContracts);

			Partners = new JoinCollectionFacade<Partner, Contract, PartnerContract>(this, PartnerContracts);
		}
		

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		[NotMapped]
		public ICollection<Type> Types { get; }
		private ICollection<ContractType> ContractTypes { get; } = new List<ContractType>();

		[NotMapped]
		public ICollection<Department> Departments { get; }
		private ICollection<DepartmentContract> DepartmentContracts { get; } = new List<DepartmentContract>();

		[NotMapped]
		public ICollection<Partner> Partners { get; }
		public ICollection<PartnerContract> PartnerContracts { get; } = new List<PartnerContract>();

		

		
	}



	
}
