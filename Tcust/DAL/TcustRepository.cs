using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;

namespace Tcust.DAL
{
	public interface ITcustRepository<T> : IAsyncRepository<T>, IRepository<T> where T : BaseEntity
	{

	}

	public class TcustRepository<T> : EfRepository<T> where T : BaseEntity
	{
		public TcustRepository(TcustContext context) : base(context)
		{

		}
	}
}
