using ApplicationCore.Specifications;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Specifications
{
    public class CategoryFilterSpecification : BaseSpecification<Category>
	{
		public CategoryFilterSpecification() : base(c => c.Active)
		{


		}
	}
}
