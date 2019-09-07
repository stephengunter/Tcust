using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Tcust.Models;
using ApplicationCore.Paging;
using System.Threading.Tasks;
using Tcust.Views;

namespace Tcust.Helpers
{
    public class ViewService
    {
        public static IEnumerable<TermYear> GetOrdered(IEnumerable<TermYear> termYears)
        {
            return termYears.OrderByDescending(y => y.Active).ThenByDescending(y => y.Year);
        }
        public static IEnumerable<Term> GetOrdered(IEnumerable<Term> terms)
        {
            return terms.OrderByDescending(t => t.Number);
        }

        public static PagedList<TermYear, TermYearViewModel> GetTermYearPagedList(IEnumerable<TermYear> termYears, int page, int pageSize)
        {
            var pageList = new PagedList<TermYear, TermYearViewModel>(termYears, page, pageSize);

            pageList.ViewList = new List<TermYearViewModel>();
            foreach (var item in pageList.List)
            {
                pageList.ViewList.Add(MapTermYearViewModel(item));
            }

            return pageList;
        }
        public static PagedList<Term, TermViewModel> GetTermsPagedList(IEnumerable<Term> terms, int page, int pageSize)
        {
            var pageList = new PagedList<Term, TermViewModel>(terms, page, pageSize);

            pageList.ViewList = new List<TermViewModel>();
            foreach (var item in pageList.List)
            {
                pageList.ViewList.Add(MapTermViewModel(item));
            }

            return pageList;
        }

        public static TermYearViewModel MapTermYearViewModel(TermYear termYear)
        {
            var model = new TermYearViewModel
            {
                id = termYear.Id,
                active = termYear.Active,
                title = termYear.Title,
                year = termYear.Year
            };
            return model;


        }

        public static TermViewModel MapTermViewModel(Term term)
        {
            var model = new TermViewModel
            {
                id = term.Id,
                active = term.Active,
                title = term.Title,
                number = term.Number,
                termYearId = term.TermYearId,

            };

            if (term.TermYear != null) model.termYear = MapTermYearViewModel(term.TermYear);


            return model;


        }

        public static int CreateTermNumber(int year, int order)
        {
            string val = year.ToString() + order.ToString();
            return Convert.ToInt32(val);
        }

        public static IEnumerable<Department> GetOrdered(IEnumerable<Department> departments)
        {
            return departments.OrderBy(d => d.Code);
        }

        public static DepartmentViewModel MapDepartmentViewModel(Department department)
        {
            var model = new DepartmentViewModel
            {
                id = department.Id,
                active = department.Active,
                name = department.Name,
                code = department.Code
            };
            return model;


        }
    }
}
