using Infrastructure.Data;
using ApplicationCore.Interfaces;

namespace IdentityApp.Data
{
	public interface IIdentityRepository<T> : IAsyncRepository<T>, IRepository<T> where T : class
	{

	}

	public class IdentityRepository<T> : EfRepository<T>, IIdentityRepository<T> where T : class
	{
		public IdentityRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
