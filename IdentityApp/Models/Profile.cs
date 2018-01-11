using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace IdentityApp.Models
{
	public class Profile:BaseRecord
	{
		
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		
		public string SID { get; set; }
		public DateTime DOB { get; set; }

		public string Fullname { get; set; }
		public bool Gender { get; set; }

		
	}
}
