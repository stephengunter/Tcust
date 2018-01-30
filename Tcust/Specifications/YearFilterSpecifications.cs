using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;
namespace Tcust.Specifications
{
	public  class BaseYearFilterSpecifications : BaseSpecification<TermYear>
	{
		public BaseYearFilterSpecifications()
		{
			Criteria = t => t.Id > 0 ;
			AddInclude(t => t.Terms);
		}
	}

	public class YearIdFilterSpecifications : BaseYearFilterSpecifications
	{
		
		
		public YearIdFilterSpecifications(int id)
		{
			var compiled = Criteria.Compile();
			Criteria = t => compiled(t) && t.Id == id;
			
		}
	}

	public class YearFilterSpecifications : BaseYearFilterSpecifications
	{
		public YearFilterSpecifications(bool active)
		{
			var compiled = Criteria.Compile();
			Criteria = t => compiled(t) && t.Active == active;
		}

		public YearFilterSpecifications(int year)
		{
			var compiled = Criteria.Compile();
			Criteria = t => compiled(t) && t.Year == year;
		}
	}
}
