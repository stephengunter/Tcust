using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;
using System.Linq;


namespace Tcust.Specifications
{
    public class DepartmentFilterSpecifications : BaseSpecification<Department>
	{
		public DepartmentFilterSpecifications(string code)
		{
			Criteria = d => d.Code == code;
		}

        public DepartmentFilterSpecifications(IEnumerable<string> codes)
        {
            Criteria = d => codes.Contains(d.Code);
        }

    }

	

}
