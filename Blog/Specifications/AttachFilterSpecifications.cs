using ApplicationCore.Specifications;
using Blog.Models;

namespace Blog.Specifications
{

	public class AttachFilterSpecification : BaseSpecification<UploadFile>
	{
		public AttachFilterSpecification(int postId) : base(a => a.PostId == postId)
		{


		}

		public AttachFilterSpecification(int postId,string name) : base(a => a.PostId == postId && a.Name==name)
		{


		}
	}
}
