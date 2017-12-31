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

		
		public void SetBaseRecord(string updatedBy, DateTime? createdAt=null)
		{
			this.UpdatedBy = updatedBy;

			if (createdAt.HasValue) this.CreatedAt = (DateTime)createdAt;
			else this.CreatedAt = DateTime.Now;

			this.LastUpdated= DateTime.Now;
		}


	}
}
