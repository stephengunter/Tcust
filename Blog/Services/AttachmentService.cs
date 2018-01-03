
using Blog.Models;
using Blog.DAL;
using Blog.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using System.Linq;

namespace Blog.Services
{
	public interface IAttachmentService
	{
		UploadFile FindByName(string name, int postId);
		void Update(UploadFile attachment);
	}

	public class AttachmentService: IAttachmentService
	{
		private readonly IBlogRepository<UploadFile> uploadFileRepository;

		public AttachmentService(IBlogRepository<UploadFile> uploadFileRepository)
		{
			this.uploadFileRepository = uploadFileRepository;
		}

		public UploadFile FindByName(string name,int postId)
		{
			var filter = new AttachFilterSpecification(postId,name);

			return  uploadFileRepository.GetSingleBySpec(filter);
		}

		public void Update(UploadFile attachment)
		{
			uploadFileRepository.Update(attachment);
		}
	}
}
