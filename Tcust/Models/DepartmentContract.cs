using ApplicationCore.Entities;

namespace Tcust.Models
{
	public class DepartmentContract : IJoinEntity<Department>, IJoinEntity<Contract>
	{
		public int DepartmentId { get; set; }
		public Department Department { get; set; }
		Department IJoinEntity<Department>.Navigation
		{
			get => Department;
			set => Department = value;
		}

		public int ContractId { get; set; }
		public Contract Contract { get; set; }
		Contract IJoinEntity<Contract>.Navigation
		{
			get => Contract;
			set => Contract = value;
		}
	}

	
}
