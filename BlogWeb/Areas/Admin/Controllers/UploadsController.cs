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
	
    public class UploadsController : BaseAdminController
	{
		private readonly IAttachmentService attachmentService;

		public UploadsController(IHostingEnvironment environment, IAttachmentService attachmentService) : base(environment)
		{
			this.attachmentService = attachmentService;

		}

		[HttpPost]
		public async Task<IActionResult> Store(UploadForm form)
		{
			
			foreach (var file in form.files)
			{
				if (file.Length > 0)
				{
					var attachment = attachmentService.FindByName(file.FileName, form.postId);
					if (attachment == null) throw new Exception(String.Format("attachmentService.FindByName({0},{1})", file.FileName, form.postId));

					

					var image = Image.FromStream(file.OpenReadStream());
					attachment.Width = image.Width;
					attachment.Height = image.Height;
				
					var saveFile = await SaveFile(file);
					attachment.Type = saveFile.Type;
					attachment.Path = saveFile.Path;

					attachmentService.Update(attachment);
				}
			}



			return   new NoContentResult(); 
			
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
				 Path= folderName + "/" + fileName
			};

			return entity;
		}

		 
    }
}