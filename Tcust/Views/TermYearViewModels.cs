using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;

using System.ComponentModel.DataAnnotations;
using ApplicationCore.Views;

namespace Tcust.Views
{
	public class TermYearViewModel
	{
		public int id { get; set; }

		[Required(ErrorMessage = "請填寫年度")]
		[Range(0, int.MaxValue, ErrorMessage = "必須為整數")]
		public int year { get; set; }

		public string title { get; set; }
		public bool active { get; set; }

		public TermYear MapToEntity(TermYear entity = null)
		{
			if (entity == null) entity = new TermYear();

			entity.Year = year;
			entity.Title = title;

			return entity;
		}
	}

	public class TermViewModel
	{
		public int id { get; set; }
		public int termYearId { get; set; }

		public bool active { get; set; }

		public string title { get; set; }
		public int number { get; set; }

		public TermYearViewModel termYear { get; set; }


		public Term MapToEntity(int number,Term entity = null)
		{
			if (entity == null) entity = new Term();

			entity.Number = number;
			entity.TermYearId = termYearId;
			entity.Active = active;
			entity.Title = title;

			return entity;
		}
	}


	public class TermYearEditForm
	{
		public TermYearViewModel year { get; set; }
	}

	public class TermEditForm
	{
		public TermViewModel term { get; set; }
		public int order { get; set; }
		public List<BaseOption> yearOptions{ get; set; }
	}
}
