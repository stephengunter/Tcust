using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
	public interface IAsyncRepository<T> where T : class
	{
		Task<T> GetByIdAsync(int id);
		Task<List<T>> ListAllAsync();
		Task<List<T>> ListAsync(ISpecification<T> spec);

		Task<T> GetAsync(Expression<Func<T, bool>> criteria);
		Task<List<T>> GetManyAsync(Expression<Func<T, bool>> criteria);

		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);


		
	}
}
