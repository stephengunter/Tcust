using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Http;

namespace BlogWeb.Models
{
	public class MediaViewModel
	{
		public MediaViewModel() { }

		public MediaViewModel(UploadFile file)
		{
			this.id = file.Id;
			this.postId = file.PostId;
			this.name = file.Name;
			this.title = file.Title;
			this.order = file.Order;
		
			this.width = file.Width;
			this.height = file.Height;
		
			this.path = file.Path;
		}

		public int id { get; set; }

		public int postId { get; set; }

		public int order { get; set; }

		public string path { get; set; }

		public string name { get; set; }

		public string title { get; set; }


		public int width { get; set; }

		public int height { get; set; }



		public UploadFile MapToEntity(string updatedBy , UploadFile entity = null)
		{
			if(entity==null) entity = new UploadFile();

			entity.PostId = postId;
			entity.Name = name;
			entity.Title = title;
			entity.Order = order;

			entity.SetUpdated(updatedBy);


			return entity;
		}



	}

	public class UploadForm
	{
		public int postId { get; set; }
		public List<IFormFile> files { get; set; }
	}
}
