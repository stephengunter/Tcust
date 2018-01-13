using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;
using IdentityServer4.EntityFramework.DbContexts;

namespace IdentityApp.Areas.Admin.Data
{
	public interface IIdentityConfigRepository<T> : IAsyncRepository<T>, IRepository<T> where T : class
	{

	}

	public class IdentityConfigRepository<T> : EfRepository<T>, IIdentityConfigRepository<T> where T : class
	{
		public IdentityConfigRepository(ConfigurationDbContext context) : base(context)
		{

		}
	}
}
