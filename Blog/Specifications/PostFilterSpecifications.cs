using ApplicationCore.Specifications;
using Blog.Models;
using ApplicationCore.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Blog.Specifications
{
	public class TopPostFilterSpecification : BaseSpecification<Post>
	{
		public TopPostFilterSpecification(bool top) : base(p => p.Top == top && p.CreateYear >= 2013 )
		{
			AddInclude(p => p.Attachments);

		}
	}


	public class PostIdFilterSpecification : BaseSpecification<Post>
	{
		public PostIdFilterSpecification(int id) : base(p => p.Id == id)
		{
			AddInclude(p => p.Attachments);
			
		}
	}

	

	public class PostFilterSpecification : BaseSpecification<Post>
	{
		public PostFilterSpecification() : base(p => p.CreateYear >= 2013)
		{
			AddInclude(p => p.Attachments);
		}

		public PostFilterSpecification(string keyword) : base(
				p => p.CreateYear >= 2013 && (p.Title != null && p.Title.CaseInsensitiveContains(keyword) ||
											  p.Author != null && p.Author.CaseInsensitiveContains(keyword) ||
											  p.Content != null && p.Content.CaseInsensitiveContains(keyword)
											 )
			)
		{

			AddInclude(p => p.Attachments);


		}


		public PostFilterSpecification(int year) : base(p=> p.CreateYear == year)
		{

			AddInclude(p => p.Attachments);
		}

		public PostFilterSpecification(int year,int month) : base(p => p.CreateYear == year && p.CreateMonth == month)
		{
			AddInclude(p => p.Attachments);
		}


	}
}
