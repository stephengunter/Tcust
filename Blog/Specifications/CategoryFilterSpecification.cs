using ApplicationCore.Specifications;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Specifications
{
    public class CategoryFilterSpecification : BaseSpecification<Category>
	{
		public CategoryFilterSpecification() 
		{
			Criteria = c => c.Active;
		}

		public CategoryFilterSpecification(IList<int> ids)
		{
			Criteria = c => ids.Contains(c.Id);
		}
	}

	
}
