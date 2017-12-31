using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;

namespace Blog.DAL
{
	public interface IBlogRepository<T>: IAsyncRepository<T> , IRepository<T> where T : BaseEntity
	{

	}

	public class BlogRepository<T> : EfRepository<T>, IBlogRepository<T>  where T : BaseEntity
	{
		public BlogRepository(BlogContext context) : base(context)
		{

		}
	}
}
