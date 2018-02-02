using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApp.Models
{
	public class ApplicationUser : IdentityUser
	{
		public Profile Profile { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime LastUpdated { get; set; }
		public string UpdatedBy { get; set; }

	
		public void SetUpdated(string updatedBy)
		{
			this.UpdatedBy = updatedBy;
			this.LastUpdated = DateTime.Now;
		}

		
		
	}
}
