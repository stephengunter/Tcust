
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;

namespace Blog.Services
{
	public interface IPostService
	{
		Post Create(Post post);
		void Update(Post post);
		void Delete(Post post);
		Post GetById(int id);

		Task<IEnumerable<Post>> GetAllAsync();

		IEnumerable<Post> GetAll();

		Task<Post> GetByIdAsync(int id);

		Task<IEnumerable<Post>> GetByKeywordAsync(string keyword);

		Task<int> CheckYearAsync(int year, IEnumerable<Post> postList = null);

		Task<IEnumerable<Post>> GetByYearAsync(int year, IEnumerable<Post> postList = null);

		Task<IEnumerable<Post>> GetByYearMonthAsync(int year, int month);

	}

	public class PostService: IPostService
	{
		private readonly IBlogRepository<Post> postRepository;
		private readonly IBlogRepository<UploadFile> uploadFileRepository;

		public PostService(IBlogRepository<Post> postRepository, IBlogRepository<UploadFile> uploadFileRepository)
		{
			this.postRepository = postRepository;
			this.uploadFileRepository = uploadFileRepository;
		}

		public async Task<IEnumerable<Post>> GetAllAsync() 
		{
			var allpost = await postRepository.ListAllAsync();
			return allpost.Where(p => p.CreateYear >= 2013);
		}

		public IEnumerable<Post> GetAll()
		{
			var filter = new PostFilterSpecification();
			return  postRepository.List(filter);
		}

		public Post GetById(int id)
		{
			var filter = new PostIdFilterSpecification(id);

			return postRepository.GetSingleBySpec(filter);

		}

		

		public async Task<Post> GetByIdAsync(int id) => await postRepository.GetByIdAsync(id);
		

		

		public async Task<IEnumerable<Post>> GetByKeywordAsync(string keyword)
		{
			var filter = new PostFilterSpecification(keyword);

			return await postRepository.ListAsync(filter);
		}

		public async Task<int> CheckYearAsync(int year, IEnumerable<Post> postList = null)
		{
			if (postList.IsNullOrEmpty()) postList = await GetAllAsync();

			var source = postList.Select(p => p.CreateYear).Distinct();
			if (!source.Contains(year)) year = source.Max();

			return year;
		}

		public async Task<IEnumerable<Post>> GetByYearAsync(int year, IEnumerable<Post> postList = null)
		{
			var filter = new PostFilterSpecification(year);

			return await postRepository.ListAsync(filter);
		}

		public async Task<IEnumerable<Post>> GetByYearMonthAsync(int year, int month)
		{
			var filter = new PostFilterSpecification(year, month);

			var posts = await postRepository.ListAsync(filter);

			return posts;
		}

		public IEnumerable<UploadFile> GetPostAttachments(int postId)
		{
			var filter = new AttachFilterSpecification(postId);
			return uploadFileRepository.List(filter);
		}

		public Post Create(Post post)
		{
			postRepository.Add(post);
			return post;
		}

		public void Update(Post post)
		{
			var oldFiles = GetPostAttachments(post.Id);
			uploadFileRepository.DeleteRange(oldFiles);

			postRepository.Update(post);
		}

		public void Delete(Post post)
		{
			postRepository.Delete(post);
		}
	}
}
