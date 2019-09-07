using ApplicationCore.Paging;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;

namespace Blog.Views
{
	public class TargetIndexViewModel
	{
		public PagedList<DepartmentTarget, TargetViewModel> TargetViewList  { get; set; }

		public PagedList<Department, TargetViewModel> MonthlyViewList { get; set; }

		public List<int> Monthes { get; set; }

		public TargetIndexViewModel()
		{
			Monthes = new List<int>();
		}
	}


	public class TargetViewModel
    {
		public int id { get; set; }
		public string departmentName { get; set; }
		public int departmentId { get; set; }
		public int target { get; set; }

		public int total { get; set; }
		public int sub { get; set; }

		public int newTarget { get; set; }

		public IList<int> monthPostCounts { get; set; }
	}

	public class TargetEditForm
	{
		public int id { get; set; }
		public int target { get; set; }

	}
}
