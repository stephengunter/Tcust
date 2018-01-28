using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tcust.DAL;
using Tcust.Models;
using Tcust.Specifications;

namespace Tcust.Services
{
	public interface ITermService
	{
		Task<IEnumerable<TermYear>> GeTermYearsAsync();
	}

	public class TermService: ITermService
	{
		private readonly ITcustRepository<TermYear> termYearRepository;
		private readonly ITcustRepository<Term> termRepository;
		

		public TermService(ITcustRepository<TermYear> termYearRepository, ITcustRepository<Term> termRepository)
		{
			this.termYearRepository = termYearRepository;
			this.termRepository = termRepository;
		}


		public async Task<IEnumerable<TermYear>> GeTermYearsAsync()
		{
			var filter = new BaseTermYearFilterSpecifications();
			return await termYearRepository.ListAsync(filter);
		}

	}
}
