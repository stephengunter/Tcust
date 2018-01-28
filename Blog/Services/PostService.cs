
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;
using System;
using ApplicationCore.Views;

namespace Blog.Services
{
	public interface IPostService
	{
		Task<Post> CreateAsync(Post post);
		Task UpdateAsync(Post post, List<int> categoryIds);

		Task UpdateAsync(Post post);

		Task DeleteAsync(int id, string updatedBy);

		Post GetById(int id);
		IEnumerable<Post> ListByIds(IList<int> ids);

		Task<IEnumerable<Post>> FetchPosts(Category category = null, bool reviewed = true, string keyword = "");

		Task ReviewPosts(IList<int> ids);

		Task<IEnumerable<Post>> GetAllAsync();

		IEnumerable<Post> GetAll();

		Task<Post> GetByIdAsync(int id);

		Task<IEnumerable<Post>> GetByKeywordAsync(string keyword);

		Task<int> CheckYearAsync(int year, IEnumerable<Post> postList = null);

		Task AddClick (int postId);
		Task<int> GetPostClickCount(int postId);

		List<PostClickModel> GroupPostByClicks(bool desc = true);
		List<PostClickModel> GroupPostByClicksInPeriod(DateTime begin, DateTime end, bool desc = true);

		Task<IEnumerable<Category>> GetCategoriesAsync(bool excludeDefault = false);
		Task<Category> GetCategoryByIdAsync(int id, bool returnDefault = false);
		Category GetCategoryByCode(string code);
		Task<IList<int>> GetCategoryIdsAsync(int postId);
		Task<IEnumerable<Category>> GetPostCategoriesAsync(Post post);



	}

	public class PostService : IPostService
	{
		private readonly IBlogRepository<Post> postRepository;
		private readonly IBlogRepository<Category> categoryRepository;
		private readonly IBlogRepository<UploadFile> uploadFileRepository;
		private readonly IBlogRepository<Click> clickRepository;
		private readonly IBlogRepository<PostCategory> postsCategoriesRepository;


		public PostService(IBlogRepository<Post> postRepository, IBlogRepository<Category> categoryRepository,
			IBlogRepository<Click> clickRepository, IBlogRepository<UploadFile> uploadFileRepository , IBlogRepository<PostCategory> postsCategoriesRepository)
		{
			this.postRepository = postRepository;
			this.categoryRepository = categoryRepository;
			this.uploadFileRepository = uploadFileRepository;
			this.clickRepository = clickRepository;
			this.postsCategoriesRepository = postsCategoriesRepository;
		}

		public async Task<IEnumerable<Post>> GetAllAsync()
		{
			var filter = new BasePostFilterSpecification();
			return await postRepository.ListAsync(filter);

		}

		public async Task<IEnumerable<Post>> FetchPosts(Category category = null, bool reviewed = true, string keyword = "")
		{
			Task<IEnumerable<Post>> getPostsTask;
			if (String.IsNullOrEmpty(keyword))
			{
				getPostsTask = GetAllAsync();
			}
			else
			{
				getPostsTask = GetByKeywordAsync(keyword);
			}

			var posts = await getPostsTask;

			if (category!=null)
			{
				var idsInCategory = await GetPostIdsInCategory(category);
				posts = posts.Where(p => idsInCategory.Contains(p.Id));

			}

			return posts.Where(p=>p.Reviewed== reviewed);



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
		public IEnumerable<Post> ListByIds(IList<int> ids)
		{
			var filter = new PostIdFilterSpecification(ids);
			return postRepository.List(filter);
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

			var source = postList.Select(p => p.Year).Distinct();
			if (!source.Contains(year)) year = source.Max();

			return year;
		}

		

		public IEnumerable<UploadFile> GetPostAttachments(int postId)
		{
			var filter = new AttachFilterSpecification(postId);
			return uploadFileRepository.List(filter);
		}

		public async Task<Post> CreateAsync(Post post)
		{
			return await postRepository.AddAsync(post);			
		
		}

		public async Task UpdateAsync(Post post, List<int> categoryIds)
		{
			await postRepository.UpdateAsync(post);

			await SyncPostCategories(post, categoryIds);

		}
		public async Task UpdateAsync(Post post)
		{
			await postRepository.UpdateAsync(post);

		}

		public async Task ReviewPosts(IList<int> ids)
		{
			foreach (var id in ids)
			{
				var post = await GetByIdAsync(id);
				post.Reviewed = true;
				await postRepository.UpdateAsync(post);
			}
		}

		public async Task DeleteAsync(int id, string updatedBy)
		{
			var post = postRepository.GetById(id);
			post.Removed = true;
			post.SetUpdated(updatedBy);

			await postRepository.UpdateAsync(post);

		}
		public async Task AddClick(int postId)
		{
			var click = new Click { DateTime = DateTime.Now, PostId = postId };
			await clickRepository.AddAsync(click);
		}
		public async Task<int> GetPostClickCount(int postId)
		{
			var filter = new ClickFilterSpecification(postId);
			var clicks = await clickRepository.ListAsync(filter);

			return clicks.Count();
		}

		public List<PostClickModel> GroupPostByClicksInPeriod(DateTime begin , DateTime end, bool desc = true )
		{
			
			var result = clickRepository.DbSet.Where(c => !c.Post.Removed)
								.Where(c=>c.DateTime.Date >= begin.Date && c.DateTime.Date <= end.Date)
								.GroupBy(p => new { PostId = p.PostId })
								.Select(d => new { postId = d.Key.PostId, count = d.Count() });


			if (desc) result = result.OrderByDescending(g => g.count);
			else result = result.OrderBy(g => g.count);



			return result.Select(c => new PostClickModel(c.postId, c.count)).ToList();

		}
		public List<PostClickModel> GroupPostByClicks(bool desc=true)
		{
			var result = clickRepository.DbSet.Where(c=>!c.Post.Removed)
												.GroupBy(p => new { PostId = p.PostId })
												.Select(d => new { postId = d.Key.PostId, count = d.Count() });
								

			if(desc) result= result.OrderByDescending(g => g.count);
			else result = result.OrderBy(g => g.count);



			return result.Select(c => new PostClickModel(c.postId, c.count)).ToList();
			
		}

		



		public async Task<IEnumerable<Category>> GetCategoriesAsync(bool excludeDefault=false)
		{
			var filter = new BaseCategoryFilterSpecification();
			var categories= await categoryRepository.ListAsync(filter);

			if (excludeDefault) categories = categories.Where(c => c.Code != "diary").ToList();


			return categories.Where(c=>c.Active).OrderByDescending(c=>c.Order);
		}
		public async Task<Category> GetCategoryByIdAsync(int id, bool returnDefault=false)
		{
			var category = await categoryRepository.GetByIdAsync(id);
			if (category != null) return category;

			if (!returnDefault) return category;

			var  categories = await GetCategoriesAsync();
			return categories.OrderByDescending(c=>c.Order).FirstOrDefault();
		}

		public Category GetCategoryByCode(string code)
		{
			var filter =  new CategoryFilterSpecification(code);
			return  categoryRepository.GetSingleBySpec(filter);
		}


		public async Task<IList<int>> GetCategoryIdsAsync(int postId)
		{
			var post = await GetByIdAsync(postId);
			
			return await GetCategoryIdsInPost(post);
		}

		
		public async Task<IEnumerable<Category>> GetPostCategoriesAsync(Post post)
		{
			var categoryIds= await GetCategoryIdsInPost(post);
			var filter = new CategoryFilterSpecification(categoryIds);
			return await categoryRepository.ListAsync(filter);
		}


		private async Task SyncPostCategories(Post post, IList<int> categoryIds)
		{
			
			var current = await GetCategoryIdsInPost(post);

			var needRemoveIds = current.Where(i => !categoryIds.Contains(i));
			if (!needRemoveIds.IsNullOrEmpty())
			{
				var spec = new PostCategoryFilterSpecification(post, needRemoveIds.ToList());
				var removeItems = await postsCategoriesRepository.ListAsync(spec);

				postsCategoriesRepository.DeleteRange(removeItems);
			}

			var needToAdd = categoryIds.Where(i => !current.Contains(i));
			if (!needToAdd.IsNullOrEmpty())
			{
				foreach (var newCategoryId in needToAdd)
				{
				   await postsCategoriesRepository.AddAsync(new PostCategory { PostId = post.Id, CategoryId = newCategoryId });
				
				}
			}

		}


		private async Task<IList<int>> GetPostIdsInCategory(Category category)
		{
			var filter = new PostCategoryFilterSpecification(category);

			var postCategories = await postsCategoriesRepository.ListAsync(filter);

			return postCategories.Select(pc => pc.PostId).ToList();
		

		}

		private async Task<IList<int>> GetCategoryIdsInPost(Post post)
		{
			var filter = new PostCategoryFilterSpecification(post);

			var postCategories = await postsCategoriesRepository.ListAsync(filter);

			return postCategories.Select(pc => pc.CategoryId).ToList();
		}

		

	}
}
