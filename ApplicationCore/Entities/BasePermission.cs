using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public abstract class BasePermission:BaseEntity
    {
		public string Name { get; set; }
		public string Title { get; set; }
	}

	public abstract class BaseUserPermission : BaseRecord
	{
		public string UserId { get; set; }

		
	}
}
