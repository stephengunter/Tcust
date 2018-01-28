using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;

namespace Tcust.DAL
{
	public interface ITcustRepository<T> : IAsyncRepository<T>, IRepository<T> where T : class
	{

	}

	
	public class TcustRepository<T> : EfRepository<T>, ITcustRepository<T> where T : class
	{
		public TcustRepository(TcustContext context) : base(context)
		{

		}
	}
}
