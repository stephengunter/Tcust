using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;

namespace Permissions.Models
{
	public class Permission : BaseEntity
	{
		public string Name { get; set; }
		public string Title { get; set; }


		public bool Removed { get; set; }
	}
}
