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
	public class PostDepartmentFilterSpecification : BaseSpecification<PostDepartment>
	{
		public PostDepartmentFilterSpecification(int postId, int departmentId)
		{
			Criteria = pc => pc.DepartmentId == departmentId && pc.PostId == postId;
		}

		public PostDepartmentFilterSpecification(Post post)
		{
			Criteria = pc => pc.PostId == post.Id;
		}

		public PostDepartmentFilterSpecification(Department department)
		{
			Criteria = pc => pc.DepartmentId == department.Id;
		}

		public PostDepartmentFilterSpecification(Post post, IList<int> departmentIds)
		{

			Criteria = pc => pc.PostId == post.Id && departmentIds.Contains(pc.DepartmentId);
		}

        public PostDepartmentFilterSpecification(IList<int> departmentIds)
        {

            Criteria = pc => departmentIds.Contains(pc.DepartmentId);
        }
    }

}
