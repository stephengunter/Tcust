using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Views
{
    public class PostClickModel
    {
		public PostClickModel(int postId, int clickCount)
		{
			this.postId = postId;
			this.clickCount = clickCount;
		}
		public int postId { get; set; }
		public int clickCount { get; set; }
	}
}
