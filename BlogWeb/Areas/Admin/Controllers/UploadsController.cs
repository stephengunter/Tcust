using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Blog.Models;
using Blog.Services;
using ApplicationCore.Helpers;
using BlogWeb.Models;
using ApplicationCore.Paging;

using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace BlogWeb.Areas.Admin.Controllers
{
	public class UploadModel
	{
		public int id { get; set; }
		public List<UploadFile> attachments { get; set; }
		public List<IFormFile> images { get; set; }
	}
    public class UploadsController : BaseAdminController
	{
		public UploadsController(IHostingEnvironment environment) : base(environment)
		{
			
	    }

		[HttpPost]
		public async Task<List<UploadFile>> Store(UploadForm form)
		{
			/*List<IFormFile> files, */
			var entityList = new List<UploadFile>();

			
			//foreach (var file in files)
			//{
			//	if (file.Length > 0)
			//	{
			//		//var image = Image.FromStream(file.OpenReadStream());

			//		var entity = await SaveFile(file);
			//		//entity.Width = image.Width;
			//		//entity.Height = image.Height;

			//		entityList.Add(entity);
			//	}
			//}

			return entityList;
			
		}

		private async Task<UploadFile> SaveFile(IFormFile file)
		{
			//檢查檔案路徑
			string folderName = DateTime.Now.ToString("yyyyMM");
			string folderPath = Path.Combine(this.UploadFilesPath, folderName);
			if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

			string extension= Path.GetExtension(file.FileName);

			string fileName = String.Format("{0}{1}", Guid.NewGuid(), extension);  
			string filePath = Path.Combine(folderPath, fileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}


			var entity = new UploadFile()
			{
				 Type= extension,
				 Name= file.FileName,
				 Path= folderName + "/" + fileName
			};

			return entity;
		}

		 
    }
}