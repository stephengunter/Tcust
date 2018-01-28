using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;

namespace Tcust.Specifications
{
    public class BaseTermYearFilterSpecifications : BaseSpecification<TermYear>
	{
		public BaseTermYearFilterSpecifications()
		{
			Criteria = t => t.Id > 0;

			AddInclude(t => t.Terms);
		}
	}
}
