using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;

namespace Blog.Models
{
    public class Click : BaseEntity
    {
		public int PostId { get; set; }
		public Post Post { get; set; }
		public DateTime DateTime { get; set; }

	}
}
