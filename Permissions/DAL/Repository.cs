using Infrastructure.Data;
using ApplicationCore.Interfaces;

namespace Permissions.DAL
{
	public interface IPermissionRepository<T> : IAsyncRepository<T>, IRepository<T> where T : class
	{

	}

	public class PermissionRepository<T> : EfRepository<T>, IPermissionRepository<T> where T : class
	{
		public PermissionRepository(PermissionContext context) : base(context)
		{

		}
	}
}
