using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
	
	/// <summary>
	/// "There's some repetition here - couldn't we have some the sync methods call the async?"
	/// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class EfRepository<T> : IRepository<T>, IAsyncRepository<T> where T : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;

		public EfRepository(DbContext dbContext)
		{
			this._dbContext = dbContext;
			this._dbSet = dbContext.Set<T>();
		}

		public DbSet<T> DbSet { get { return _dbSet; } }

		public void Save()
		{
			_dbContext.SaveChanges();
		}


		public virtual T GetById(int id)
		{
			return _dbSet.Find(id);
		}

		public T GetSingleBySpec(ISpecification<T> spec)
		{
			return List(spec).FirstOrDefault();
		}


		public virtual async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public IEnumerable<T> ListAll()
		{
			
			return _dbSet.AsEnumerable();
		}

		public async Task<List<T>> ListAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public IEnumerable<T> List(ISpecification<T> spec)
		{
			// fetch a Queryable that includes all expression-based includes
			var queryableResultWithIncludes = spec.Includes
				.Aggregate(_dbSet.AsQueryable(),
					(current, include) => current.Include(include));

			// modify the IQueryable to include any string-based include statements
			var secondaryResult = spec.IncludeStrings
				.Aggregate(queryableResultWithIncludes,
					(current, include) => current.Include(include));

			// return the result of the query using the specification's criteria expression
			return secondaryResult.Where(spec.Criteria).AsEnumerable();

		}
		public async Task<List<T>> ListAsync(ISpecification<T> spec)
		{
			// fetch a Queryable that includes all expression-based includes
			var queryableResultWithIncludes = spec.Includes
				.Aggregate(_dbSet.AsQueryable(),
					(current, include) => current.Include(include));

			// modify the IQueryable to include any string-based include statements
			var secondaryResult =  spec.IncludeStrings
				.Aggregate(queryableResultWithIncludes,
					(current, include) => current.Include(include));

			// return the result of the query using the specification's criteria expression
			return await secondaryResult
							.Where(spec.Criteria).ToListAsync();

		}

		public T Add(T entity)
		{
			_dbSet.Add(entity);
			_dbContext.SaveChanges();

			return entity;
		}

		public async Task<T> AddAsync(T entity)
		{
			_dbSet.Add(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public void Update(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}
		public async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
			_dbContext.SaveChanges();
		}
		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public void UpdateRange(IEnumerable<T> entityList)
		{
			_dbSet.UpdateRange(entityList);
			_dbContext.SaveChanges();

		}

		public void DeleteRange( IEnumerable<T> entityList)
		{
			_dbSet.RemoveRange(entityList);
			_dbContext.SaveChanges();

		}

		public T Get(Expression<Func<T, bool>> criteria)
		{
			return  _dbSet.Where(criteria).FirstOrDefault();
		}

		public List<T> GetMany(Expression<Func<T, bool>> criteria)
		{
			return _dbSet.Where(criteria).ToList();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> criteria)
		{
			return await _dbSet.Where(criteria).FirstOrDefaultAsync();
		}
		public async  Task<List<T>> GetManyAsync(Expression<Func<T, bool>> criteria)
		{
			return await _dbSet.Where(criteria).ToListAsync();

		}


		
	}
}
