using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore.Interfaces
{
	public interface IRepository<T> where T : BaseEntity
	{

		DbSet<T> DbSet { get; }

		T GetById(int id);
		T GetSingleBySpec(ISpecification<T> spec);
		IEnumerable<T> ListAll();
		IEnumerable<T> List(ISpecification<T> spec);
		T Add(T entity);
		void Update(T entity);
		void Delete(T entity);

		void DeleteRange(IEnumerable<T> entityList);


		
	}
}
