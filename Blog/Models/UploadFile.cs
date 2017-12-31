
using System.Text.RegularExpressions;
using ApplicationCore.Entities;

namespace Blog.Models
{
	public class UploadFile : BsseUploadFile
	{

		public string ItemOID { get; set; }
		public bool Top { get; set; }

		public int PostId { get; set; }
		public Post Post { get; set; }

		public string ClearName
		{
			get
			{
				return Regex.Replace(this.Name, ".jpg", "", RegexOptions.IgnoreCase);
			}
		}
	}
}
