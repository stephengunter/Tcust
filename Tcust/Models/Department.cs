using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Tcust.Models
{
	public class Department : BaseRecord
	{
		public Department() => Contracts = new JoinCollectionFacade<Contract, Department, DepartmentContract>(this, DepartmentContracts);

		public string Code { get; set; }

		public string Name { get; set; }

		public bool Active { get; set; }


		public DateTime StartDate { get; set; }

		

		[NotMapped]
		public ICollection<Contract> Contracts { get; }

		private ICollection<DepartmentContract> DepartmentContracts { get; } = new List<DepartmentContract>();
	}

	

}
