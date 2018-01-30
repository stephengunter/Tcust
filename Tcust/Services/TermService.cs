using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tcust.DAL;
using Tcust.Models;
using Tcust.Specifications;
using System.Linq;

namespace Tcust.Services
{
	public interface ITermService
	{
		Task<TermYear> CreateTermYearAsync(TermYear termYear);
		Task<TermYear> GetTermYearByIdAsync(int id);
		TermYear GetTermYearById(int id);
		Task UpdateTermYearAsync(TermYear termYear);

		Task<IEnumerable<TermYear>> GetAllTermYearsAsync();
		TermYear GetActiveTermYear();

		TermYear GetTermYear(int year);

		Task<Term> CreateAsync(Term term);
		Task<Term> GetByIdAsync(int id);
		Task UpdateAsync(Term term);
		Task<IEnumerable<Term>> GetAllTermAsync();
		Term GetTermByNumber(int number);

	}

	public class TermService : ITermService
	{
		private readonly ITcustRepository<TermYear> termYearRepository;
		private readonly ITcustRepository<Term> termRepository;


		public TermService(ITcustRepository<TermYear> termYearRepository, ITcustRepository<Term> termRepository)
		{
			this.termYearRepository = termYearRepository;
			this.termRepository = termRepository;
		}
		public async Task<TermYear> CreateTermYearAsync(TermYear termYear)
		{
			return await termYearRepository.AddAsync(termYear);
		}

		public async Task UpdateTermYearAsync(TermYear termYear)
		{
			await termYearRepository.UpdateAsync(termYear);
		}

		public async Task<IEnumerable<TermYear>> GetAllTermYearsAsync()
		{
			return await termYearRepository.ListAllAsync();
		}

		public TermYear GetActiveTermYear()
		{
			bool active = true;
			var spec = new YearFilterSpecifications(active);
			return termYearRepository.GetSingleBySpec(spec);
		}
		public async Task<TermYear> GetTermYearByIdAsync(int id)
		{
			return await termYearRepository.GetByIdAsync(id);
		}
		public TermYear GetTermYearById(int id)
		{
			var spec = new YearIdFilterSpecifications(id);
			return termYearRepository.GetSingleBySpec(spec);
		}
		public TermYear GetTermYear(int year)
		{
			var spec = new YearFilterSpecifications(year);
			return termYearRepository.GetSingleBySpec(spec);
		}

		public async Task<Term> CreateAsync(Term term)
		{
			term= await termRepository.AddAsync(term);

			await SetTermYearActive(term.TermYearId);

			return term;
		}

		public async Task<Term> GetByIdAsync(int id)
		{
			return await termRepository.GetByIdAsync(id);
		}
		public async Task UpdateAsync(Term term)
		{
			await termRepository.UpdateAsync(term);

			await SetTermYearActive(term.TermYearId);
		}

		public async Task<IEnumerable<Term>> GetAllTermAsync()
		{
			var spec = new BaseTermFilterSpecifications();
			return await termRepository.ListAsync(spec);
		}

		public Term GetTermByNumber(int number)
		{
			var spec = new TermNumberFilterSpecifications(number);
			return  termRepository.GetSingleBySpec(spec);
		}


		async Task SetTermYearActive(int termYearId)
		{
			var termYear= GetTermYearById(termYearId);

			var activeTerm = termYear.Terms.Where(t => t.Active).FirstOrDefault();
		

			termYear.Active = activeTerm != null;

			await UpdateTermYearAsync(termYear);
		}
	}
}
