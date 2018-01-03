using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
	

	public abstract class BaseRecord : BaseEntity
	{
		public DateTime CreatedAt { get; set; }
		public DateTime LastUpdated { get; set; }
		public string UpdatedBy { get; set; }

		
		public void SetUpdated(string updatedBy)
		{
			this.UpdatedBy = updatedBy;
			this.LastUpdated= DateTime.Now;
		}


	}
}
