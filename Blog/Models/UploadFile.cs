
using System.Text.RegularExpressions;
using ApplicationCore.Entities;

namespace Blog.Models
{
	public class UploadFile : BsseUploadFile
	{

		
		public int Order { get; set; }

		public int PostId { get; set; }
		public Post Post { get; set; }

		
	}
}
