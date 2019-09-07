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
	public class PostIssuerFilterSpecification : BaseSpecification<PostIssuer>
	{
		public PostIssuerFilterSpecification(int postId, int departmentId)
		{
			Criteria = pc => pc.DepartmentId == departmentId && pc.PostId == postId;
		}

		public PostIssuerFilterSpecification(Post post)
		{
			Criteria = pc => pc.PostId == post.Id;
		}

		public PostIssuerFilterSpecification(Department department)
		{
			Criteria = pc => pc.DepartmentId == department.Id;
		}

		public PostIssuerFilterSpecification(Post post, IList<int> departmentIds)
		{

			Criteria = pc => pc.PostId == post.Id && departmentIds.Contains(pc.DepartmentId);
		}
	}

}
