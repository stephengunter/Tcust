using ApplicationCore.Specifications;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Specifications
{
	public class BaseCategoryFilterSpecification : BaseSpecification<Category>
	{
		public BaseCategoryFilterSpecification()
		{
			Criteria = c => c.Active;

		}
	}


	public class CategoryFilterSpecification : BaseCategoryFilterSpecification
	{
		public CategoryFilterSpecification(string code) 
		{
			var compiled = Criteria.Compile();
			Criteria = c => compiled(c) &&  c.Code==code;
		}

		public CategoryFilterSpecification(IList<int> ids)
		{
			var compiled = Criteria.Compile();
			Criteria = c => compiled(c) && ids.Contains(c.Id);
		}
	}

	
}
