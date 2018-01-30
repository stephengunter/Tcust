using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tcust.Models
{
	public class TermYear : BaseEntity
	{
		
		public int Year { get; set; }

		public string Title { get; set; }

		public bool Active { get; set; }

		public ICollection<Term> Terms { get; set; }
	}

	public class Term : BaseEntity
	{
		public int TermYearId { get; set; }
		public TermYear TermYear { get; set; }

		public bool Active { get; set; }

		public string Title { get; set; }
		public int Number { get; set; }



		[NotMapped]
		public int Year
		{
			get
			{
				string strNumber = this.Number.ToString();

				
				if (strNumber.Length == 3)
				{
					return Convert.ToInt16(strNumber.Substring(0, 2));
				}
				else if (strNumber.Length == 4)
				{
					return Convert.ToInt16(strNumber.Substring(0, 3));
				}

				return 0;
			}
		}

		[NotMapped]
		public int Order
		{
			get
			{
				string strNumber = this.Number.ToString();


				if (strNumber.Length == 3)
				{
					return Convert.ToInt16(strNumber.Substring(2, 1));
				}
				else if (strNumber.Length == 4)
				{
					return Convert.ToInt16(strNumber.Substring(3, 1));
				}

				return 0;
			}
		}
	}
}
