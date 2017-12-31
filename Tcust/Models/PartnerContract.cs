using ApplicationCore.Entities;

namespace Tcust.Models
{
	public class PartnerContract : IJoinEntity<Partner>, IJoinEntity<Contract>
	{
		public int PartnerId { get; set; }
		public Partner Partner { get; set; }
		Partner IJoinEntity<Partner>.Navigation
		{
			get => Partner;
			set => Partner = value;
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
