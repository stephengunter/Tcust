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
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace BlogWeb.Areas.Admin.Controllers
{

	public class UploadsController : BaseAdminController
	{
		private readonly IAttachmentService attachmentService;

		public UploadsController(IHostingEnvironment environment, IOptions<AppSettings> settings, IAttachmentService attachmentService) : base(environment, settings)
		{

			this.attachmentService = attachmentService;
		}
		

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store(UploadForm form)
		{

			foreach (var file in form.files)
			{
				if (file.Length > 0)
				{
					var attachment = attachmentService.FindByName(file.FileName, form.postId);
					if (attachment == null) throw new Exception(String.Format("attachmentService.FindByName({0},{1})", file.FileName, form.postId));

					var upload = await SaveFile(file);
					attachment.Type = upload.Type;
					attachment.Path = upload.Path;

					switch (upload.Type)
					{
						case ".jpg":
						case ".jpeg":
						case ".png":
						case ".gif":
							var image = Image.FromStream(file.OpenReadStream());
							attachment.Width = image.Width;
							attachment.Height = image.Height;
							attachment.PreviewPath = upload.Path;
							break;
						case ".mp4":
							//截取影片預覽圖
							string imgPath = Path.ChangeExtension(upload.Path, ".jpg");


							string videoFullPath = Path.Combine(this.UploadFilesPath, upload.Path);
							string imgFullPath = Path.Combine(this.UploadFilesPath, imgPath);

							SaveVideoImage(videoFullPath, imgFullPath);

							attachment.PreviewPath = imgPath;

							break;
					}

					

					

					attachmentService.Update(attachment);
				}
			}



			return new NoContentResult();

		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			attachmentService.Delete(id);

			return new NoContentResult();

		}

		


		private async Task<UploadFile> SaveFile(IFormFile file)
		{
			//檢查檔案路徑
			string folderName = DateTime.Now.ToString("yyyyMMdd");
			string folderPath = Path.Combine(this.UploadFilesPath, folderName);
			if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

			string extension = Path.GetExtension(file.FileName).ToLower();

			string fileName = String.Format("{0}{1}", Guid.NewGuid(), extension);
			string filePath = Path.Combine(folderPath, fileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}


			var entity = new UploadFile()
			{
				Type = extension,
				Path = folderName + "/" + fileName
			};

			return entity;
		}

		private void test()
		{

		}
		//截取影片預覽圖
		public void SaveVideoImage(string videoPath , string imgPath)
		{
			

			string ffmpegPath = Path.Combine(this.HelpersPath, "ffmpeg.exe"); 

			Process process = new Process();
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.FileName = ffmpegPath;
			process.StartInfo.Arguments = "-i " + videoPath + " -y -f image2 -t 0.010 -s 480x360 -ss 00:00:05 " + imgPath;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;

			try
			{
				process.Start();
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				process.Close();
			}

		}

	}
}