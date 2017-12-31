using ApplicationCore.Specifications;
using Blog.Models;

namespace Blog.Specifications
{
   
	public class AttachFilterSpecification : BaseSpecification<UploadFile>
	{
		public AttachFilterSpecification(int postId) : base(p => p.PostId == postId)
		{
			

		}
	}
}
