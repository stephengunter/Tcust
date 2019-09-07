
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;
using System;
using ApplicationCore.Views;
using Tcust.Models;

namespace Blog.Services
{
	public interface IPostService
	{
		Task<Post> CreateAsync(Post post);
		Task UpdateAsync(Post post, List<int> categoryIds);

		Task UpdateAsync(Post post);

		void UpdateRange(IEnumerable<Post> posts);

		Task DeleteAsync(int id, string updatedBy);

		Post GetById(int id);

		IEnumerable<Post> ListByIds(IList<int> ids);

		Task<IEnumerable<Post>> ExceptFromCategoryAsync(IEnumerable<Post> posts, Category category);

        Task<IEnumerable<Post>> FilterByDepartmentsAsync(IEnumerable<Post> posts, IEnumerable<Department> departments);

        Task<IEnumerable<Post>> FetchPosts(Category category = null, bool reviewed = true, string keyword = "");        

        Task ReviewPosts(IList<int> ids);

		Task<IEnumerable<Post>> GetAllAsync();

		IEnumerable<Post> GetAll();

		Task<Post> GetByIdAsync(int id);

		Post GetByNumber(string number);

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


		Task SyncPostDepartments(Post post, IList<int> departmentIds);
		Task SyncPostIssuers(Post post, IList<int> departmentIds);

		Task<IList<int>> GetDepartmentIdsInPostAsync(Post post);
		Task<IList<int>> GetIssuerIdsInPostAsync(Post post);

		Task<IList<int>> GetPostsIdsByDepartmentAsync(Department department);
		Task<IList<int>> GetPostsIdsByIssuerAsync(Department department);

		Task<IEnumerable<PostIssuer>> GetIssuersByPostAsync(Post post);

	}

	public class PostService : IPostService
	{
		private readonly IBlogRepository<Post> postRepository;
		private readonly IBlogRepository<Category> categoryRepository;
		private readonly IBlogRepository<UploadFile> uploadFileRepository;
		private readonly IBlogRepository<Click> clickRepository;
		private readonly IBlogRepository<PostCategory> postsCategoriesRepository;

		private readonly IBlogRepository<PostDepartment> postDepartmentRepository;
		private readonly IBlogRepository<PostIssuer> postIssuerRepository;


		public PostService(IBlogRepository<Post> postRepository, IBlogRepository<Category> categoryRepository,
			IBlogRepository<Click> clickRepository, IBlogRepository<UploadFile> uploadFileRepository , IBlogRepository<PostCategory> postsCategoriesRepository,
			IBlogRepository<PostDepartment> postDepartmentRepository, IBlogRepository<PostIssuer> postIssuerRepository)
		{
			this.postRepository = postRepository;
			this.categoryRepository = categoryRepository;
			this.uploadFileRepository = uploadFileRepository;
			this.clickRepository = clickRepository;
			this.postsCategoriesRepository = postsCategoriesRepository;

			this.postDepartmentRepository = postDepartmentRepository;
			this.postIssuerRepository = postIssuerRepository;
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
				var idsInCategory = await GetPostIdsInCategoryAsync(category);
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

		public async Task<IEnumerable<Post>> ExceptFromCategoryAsync(IEnumerable<Post> posts, Category category)
		{
			var postIds = await GetPostIdsInCategoryAsync(category);
			return posts.Where(p => !postIds.Contains(p.Id));
		}

        public async Task<IEnumerable<Post>> FilterByDepartmentsAsync(IEnumerable<Post> posts, IEnumerable<Department> departments)
        {
            var departmentsIds = departments.Select(d => d.Id).ToList();
            var spec = new PostDepartmentFilterSpecification(departmentsIds);
            var postDepartments = await postDepartmentRepository.ListAsync(spec);

            var postIds = postDepartments.Select(item => item.PostId).ToList();

            return posts.Where(p => postIds.Contains(p.Id)).ToList();

        }

        public async Task<Post> GetByIdAsync(int id) => await postRepository.GetByIdAsync(id);

		public Post GetByNumber(string number)
		{
			var filter = new PostNumberFilterSpecification(number);

			return  postRepository.GetSingleBySpec(filter);
		}


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

		public void UpdateRange(IEnumerable<Post> posts)
		{
			postRepository.UpdateRange(posts);
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


			return categories.Where(c => c.Active).OrderByDescending(c=>c.Order);
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
			
			return await GetCategoryIdsInPostAsync(post);
		}

		
		public async Task<IEnumerable<Category>> GetPostCategoriesAsync(Post post)
		{
			var categoryIds= await GetCategoryIdsInPostAsync(post);
			var filter = new CategoryFilterSpecification(categoryIds);
			return await categoryRepository.ListAsync(filter);
		}

		public async Task SyncPostDepartments(Post post, IList<int> departmentIds)
		{
			var current = await GetDepartmentIdsInPostAsync(post);

			var needRemoveIds = current.Where(i => !departmentIds.Contains(i));
			if (!needRemoveIds.IsNullOrEmpty())
			{
				var spec = new PostDepartmentFilterSpecification(post, needRemoveIds.ToList());
				var removeItems = await postDepartmentRepository.ListAsync(spec);

				postDepartmentRepository.DeleteRange(removeItems);
			}

			var needToAdd = departmentIds.Where(i => !current.Contains(i));
			if (!needToAdd.IsNullOrEmpty())
			{
				foreach (var newDepartmentId in needToAdd)
				{
					await postDepartmentRepository.AddAsync(new PostDepartment { PostId = post.Id, DepartmentId = newDepartmentId });

				}
			}

		}

		public async Task SyncPostIssuers(Post post, IList<int> departmentIds)
		{
			var current = await GetIssuerIdsInPostAsync(post);

			var needRemoveIds = current.Where(i => !departmentIds.Contains(i));
			if (!needRemoveIds.IsNullOrEmpty())
			{
				var spec = new PostIssuerFilterSpecification(post, needRemoveIds.ToList());
				var removeItems = await postIssuerRepository.ListAsync(spec);

				postIssuerRepository.DeleteRange(removeItems);
			}

			var needToAdd = departmentIds.Where(i => !current.Contains(i));
			if (!needToAdd.IsNullOrEmpty())
			{
				foreach (var newIssuerId in needToAdd)
				{
					await postIssuerRepository.AddAsync(new PostIssuer { PostId = post.Id, DepartmentId = newIssuerId });

				}
			}

		}


		private async Task SyncPostCategories(Post post, IList<int> categoryIds)
		{
			
			var current = await GetCategoryIdsInPostAsync(post);

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


		private async Task<IList<int>> GetPostIdsInCategoryAsync(Category category)
		{
			var filter = new PostCategoryFilterSpecification(category);

			var postCategories = await postsCategoriesRepository.ListAsync(filter);

			return postCategories.Select(pc => pc.PostId).ToList();
		

		}

		private async Task<IList<int>> GetCategoryIdsInPostAsync(Post post)
		{
			
			var filter = new PostCategoryFilterSpecification(post);

			var postCategories = await postsCategoriesRepository.ListAsync(filter);

			return postCategories.Select(pc => pc.CategoryId).ToList();
		}


		public async Task<IList<int>> GetDepartmentIdsInPostAsync(Post post)
		{

			var filter = new PostDepartmentFilterSpecification(post);

			var postDepartments = await postDepartmentRepository.ListAsync(filter);

			return postDepartments.Select(pc => pc.DepartmentId).ToList();
		}

		public async Task<IList<int>> GetIssuerIdsInPostAsync(Post post)
		{

			var filter = new PostIssuerFilterSpecification(post);

			var postIssuers = await postIssuerRepository.ListAsync(filter);

			return postIssuers.Select(pc => pc.DepartmentId).ToList();
		}

		public async Task<IList<int>> GetPostsIdsByDepartmentAsync(Department department)
		{
			var filter = new PostDepartmentFilterSpecification(department);

			var postDepartments = await postDepartmentRepository.ListAsync(filter);

			return postDepartments.Select(pc => pc.PostId).ToList();
		}
		public async Task<IList<int>> GetPostsIdsByIssuerAsync(Department department)
		{
			var filter = new PostIssuerFilterSpecification(department);

			var postIssuers = await postIssuerRepository.ListAsync(filter);

			return postIssuers.Select(pc => pc.PostId).ToList();
		}

		public async Task<IEnumerable<PostIssuer>> GetIssuersByPostAsync(Post post)
		{
			var filter = new PostIssuerFilterSpecification(post);

			return await postIssuerRepository.ListAsync(filter);
		}

	}
}
