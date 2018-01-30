using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;
using ApplicationCore.Paging;
using Tcust.Views;
using ApplicationCore.Views;
using System.Linq;

namespace Tcust.Helpers
{
    public static class Extentions
    {
		public static IEnumerable<TermYear> GetOrdered(this IEnumerable<TermYear> termYears)
		{
			return ViewService.GetOrdered(termYears);
		}
		public static IEnumerable<Term> GetOrdered(this IEnumerable<Term> terms)
		{
			return ViewService.GetOrdered(terms);
		}

		public static PagedList<TermYear, TermYearViewModel> ToPageList(this IEnumerable<TermYear> termYears, int page, int pageSize)
		{
			return ViewService.GetTermYearPagedList(termYears, page, pageSize);
		}
		public static PagedList<Term, TermViewModel> ToPageList(this IEnumerable<Term> terms, int page, int pageSize)
		{
			return ViewService.GetTermsPagedList(terms, page, pageSize);
		}

		public static TermYearViewModel MapTermYearViewModel(this TermYear termYear)
		{
			return ViewService.MapTermYearViewModel(termYear);
		}
		public static TermViewModel MapTermViewModel(this Term term)
		{
			return ViewService.MapTermViewModel(term);
		}

		public static List<BaseOption> ToOptions(this IEnumerable<TermYear> termYears)
		{
			var options = termYears.Select(t => new BaseOption(t.Id.ToString(), t.Year.ToString())).ToList();

			return options;
		}
	}
}
