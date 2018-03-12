using ApplicationCore.Specifications;
using Blog.Models;
using ApplicationCore.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Blog.Specifications
{

	public class BasePostFilterSpecification : BaseSpecification<Post>
	{
		public BasePostFilterSpecification()
		{
			Criteria = p => !p.Removed;// && p.Year >= 2013;

			AddInclude(p => p.Attachments);
			

		}
	}

	public class TopPostFilterSpecification : BasePostFilterSpecification
	{
		public TopPostFilterSpecification(bool top) 
		{
			var compiled = Criteria.Compile();
			Criteria = p => compiled(p) && p.Top == top;

		}
	}


	public class PostIdFilterSpecification : BasePostFilterSpecification
	{
		public PostIdFilterSpecification(int id)
		{
			var compiled = Criteria.Compile();
			Criteria = p => compiled(p) && p.Id == id;

		}
		public PostIdFilterSpecification(IList<int> ids)
		{
			var compiled = Criteria.Compile();
			Criteria = p => compiled(p) && ids.Contains(p.Id);

		}
	}

	public class PostNumberFilterSpecification : BasePostFilterSpecification
	{
		public PostNumberFilterSpecification(string number)
		{
			var compiled = Criteria.Compile();
			Criteria = p => compiled(p) && p.Number== number;
		}
	}

	public class PostFilterSpecification : BasePostFilterSpecification
	{
		

		public PostFilterSpecification(string keyword="",int year=0, int month=0)
		{

			var compiled = Criteria.Compile();


			if (!String.IsNullOrEmpty(keyword))
			{
				Criteria = p => compiled(p) && ( p.Number.CaseInsensitiveContains(keyword) ||
												p.Title != null && p.Title.CaseInsensitiveContains(keyword) ||
											  p.Author != null && p.Author.CaseInsensitiveContains(keyword) ||
											  p.Content != null && p.Content.CaseInsensitiveContains(keyword)
											 );
			}

			if (year > 0)
			{
				compiled = Criteria.Compile();
				Criteria = p => compiled(p) && p.Year == year;
			}

			if (month > 0)
			{
				compiled = Criteria.Compile();
				Criteria = p => compiled(p) && p.Month == month;
			}



		}



	}

	
}
