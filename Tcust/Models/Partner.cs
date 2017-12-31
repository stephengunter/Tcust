using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Tcust.Models
{
	public class Partner : BaseRecord
	{
		public Partner() => Contracts = new JoinCollectionFacade<Contract, Partner, PartnerContract>(this, PartnerContracts);


		public string Name { get; set; }

		public string Description { get; set; }

		public bool Active { get; set; }

		public PartnerType Type { get; set; }


		public int CountryId { get; set; }
		public int? AreaId  { get; set; }


		private ICollection<PartnerContract> PartnerContracts { get; } = new List<PartnerContract>();

		[NotMapped]
		public ICollection<Contract> Contracts { get; }

	}

	



	public enum PartnerType
	{
		School, Enterprise
	}





}
