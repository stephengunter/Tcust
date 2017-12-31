
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tcust.Models
{
    public class Type : BaseCategory
    {
		[NotMapped]
		public ICollection<Contract> Contracts { get; }

		private ICollection<ContractType> ContractTypes { get; } = new List<ContractType>();
	}

	public class ContractType : IJoinEntity<Type>, IJoinEntity<Contract>
	{
		public int ContractId { get; set; }
		public Contract Contract { get; set; }
		Contract IJoinEntity<Contract>.Navigation
		{
			get => Contract;
			set => Contract = value;
		}

		public int TypeId { get; set; }
		public Type Type { get; set; }
		Type IJoinEntity<Type>.Navigation
		{
			get => Type;
			set => Type = value;
		}
	}
}
