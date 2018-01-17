using ApplicationCore.Specifications;
using Blog.Models;

namespace Blog.Specifications
{
    
	public class ClickFilterSpecification : BaseSpecification<Click>
	{
		public ClickFilterSpecification(int postId)
		{
			Criteria = c => c.PostId == postId;
		}
	}
}
