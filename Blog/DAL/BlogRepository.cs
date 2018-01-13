using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;

namespace Blog.DAL
{
	public interface IBlogRepository<T>: IAsyncRepository<T> , IRepository<T> where T : class
	{

	}

	public class BlogRepository<T> : EfRepository<T>, IBlogRepository<T>  where T : class
	{
		public BlogRepository(BlogContext context) : base(context)
		{

		}
	}
}
