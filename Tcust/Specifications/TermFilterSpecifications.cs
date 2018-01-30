using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;

namespace Tcust.Specifications
{
	public class BaseTermFilterSpecifications : BaseSpecification<Term>
	{
		public BaseTermFilterSpecifications()
		{
			Criteria = t => t.Id > 0;

			AddInclude(t => t.TermYear);
		}
	}

	public class TermNumberFilterSpecifications:BaseTermFilterSpecifications
	{
		public TermNumberFilterSpecifications(int number)
		{
			var compiled = Criteria.Compile();
			Criteria = t => compiled(t) && t.Number == number;
		}

	}


}
