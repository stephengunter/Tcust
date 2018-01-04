
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public abstract class BaseRepository<T> where T : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;

		public BaseRepository(DbContext dbContext)
		{
			this._dbContext = dbContext;
			this._dbSet = dbContext.Set<T>();
		}

		public DbSet<T> DbSet { get { return _dbSet; } }

	}
}
