using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using System.Text;

namespace Blog.Models
{
	public class DepartmentTarget : BaseEntity
	{
		public int TermNumber { get; set; }
		public int DepartmentId { get; set; }

		public int Tatget { get; set; }

	}
}
