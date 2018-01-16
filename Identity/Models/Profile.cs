using ApplicationCore.Entities;
using System;

namespace IdentityApp.Models
{
	public class Profile : BaseRecord
	{

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }


		public string SID { get; set; }
		public DateTime DOB { get; set; }

		public string Fullname { get; set; }
		public bool Gender { get; set; }


	}
}
