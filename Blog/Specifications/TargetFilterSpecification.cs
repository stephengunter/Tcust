using ApplicationCore.Specifications;
using Blog.Models;
using ApplicationCore.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using Tcust.Models;


namespace Blog.Specifications
{
	public class TargetFilterSpecification : BaseSpecification<DepartmentTarget>
	{
		public TargetFilterSpecification(int termNumber)
		{
			Criteria = t => t.TermNumber == termNumber;
		}

		public TargetFilterSpecification(Department department, int termNumber)
		{
			Criteria = t => t.DepartmentId==department.Id && t.TermNumber == termNumber;
		}
	}
}
