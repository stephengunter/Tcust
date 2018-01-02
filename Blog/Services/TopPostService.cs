
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;
using System;
using ApplicationCore.Paging;

namespace Blog.Services
{
	public interface ITopPostService
	{
		Post GetById(int id);

		void Update(int id, string title, string summary, int cover);
		void UpdateOrder(int id, int order);

		void RemoveTopPost(int id);

		//IPagedList<Post> GetTopPosts(PagingParams pagingParams, bool top = true);

		//Task<IPagedList<Post>> GetTopPosts(PagingParams pagingParams, bool top = true);
		Task<IEnumerable<Post>> GetTopPosts(bool top = true);

	}

	public class TopPostService : ITopPostService
	{
		private readonly IBlogRepository<Post> postRepository;
		private readonly IBlogRepository<UploadFile> uploadFileRepository;

		public TopPostService(IBlogRepository<Post> postRepository, IBlogRepository<UploadFile> uploadFileRepository)
		{
			this.postRepository = postRepository;
			this.uploadFileRepository = uploadFileRepository;
		}

		//public async Task<IPagedList<Post>> GetTopPosts(PagingParams pagingParams, bool top = true)
		//{
		//	var filter = new TopPostFilterSpecification(top);
		//	var posts= await postRepository.ListAsync(filter);

			
		//	return new PagedList<Post>(
		//		posts.AsQueryable(), pagingParams.PageNumber, pagingParams.PageSize);
		//}

		public async Task<IEnumerable<Post>> GetTopPosts(bool top = true)
		{
			var filter = new TopPostFilterSpecification(top);

			return await postRepository.ListAsync(filter);
		}

		public Post GetById(int id)
		{
			var filter = new PostIdFilterSpecification(id);

			return postRepository.GetSingleBySpec(filter);
		}

		public void Update(int id, string title, string summary, int cover)
		{
			Post post = GetById(id);

			summary = summary.RemoveSciptAndHtmlTags().Trim();
			title = title.RemoveSciptAndHtmlTags().Trim();
			if (!string.IsNullOrEmpty(summary)) post.Summary = summary;

			if (!string.IsNullOrEmpty(title)) post.Title = title;

			post.Top = true;
			//if (cover > 0 && post.Attachments.Where(f => f.Id == cover).FirstOrDefault() != null)
			//{
			//	foreach (UploadFile uploadFile in post.Attachments)
			//	{
			//		int num = uploadFile.Id == cover ? 1 : 0;
			//		uploadFile.Top = num != 0;
			//	}
			//}


			postRepository.Update(post);
		}

		public void UpdateOrder(int id, int order)
		{
			Post post = GetById(id);
			post.DisplayOrder = order;
			postRepository.Update(post);
		}

		public void RemoveTopPost(int id)
		{
			Post post = GetById(id);
			post.Top = false;
			postRepository.Update(post);
		}
	}
}
