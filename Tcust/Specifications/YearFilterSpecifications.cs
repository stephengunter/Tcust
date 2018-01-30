using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;
namespace Tcust.Specifications
{
	public abstract class BaseYearFilterSpecifications : BaseSpecification<TermYear>
	{
		public BaseYearFilterSpecifications()
		{
			AddInclude(t => t.Terms);
		}
	}

	public class YearIdFilterSpecifications : BaseYearFilterSpecifications
	{
		
		
		public YearIdFilterSpecifications(int id)
		{
			Criteria = t => t.Id == id;
		}
	}

	public class YearFilterSpecifications : BaseYearFilterSpecifications
	{
		public YearFilterSpecifications(bool active)
		{
			Criteria = t => t.Active == active;
		}

		public YearFilterSpecifications(int year)
		{
			Criteria = t => t.Year == year;
		}
	}
}
