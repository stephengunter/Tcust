using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Permissions.Services;

using Blog.Models;
using Blog.Services;
using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using BlogWeb.Helpers;
using Microsoft.EntityFrameworkCore;

using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BlogWeb.Areas.Admin.Controllers
{
	[Authorize(Policy = "DEV_ONLY")]
	public class CopyController : BaseAdminController
	{
		class PostViewModel
		{

			public string id { get; set; }
			public string title { get; set; }
			public string termNumber { get; set; }
			public string number { get; set; }
			public string author { get; set; }
			public string content { get; set; }
			public string summary { get; set; }
			public string date { get; set; }
			public string updatedBy { get; set; }


			public DateTime createdAt { get; set; }
			public DateTime updatedAt { get; set; }

			public string fileIds { get; set; }

			public int categoryId { get; set; }
			public string categoryName { get; set; }

			public Post MapToEntity(Post post = null)
			{
				if (post == null)
				{
					post = new Post();
					post.Attachments = new List<UploadFile>();
				}


				post.Title = title;
				post.Content = content;
				post.TermNumber = termNumber.Trim();

				post.Author = author;

				post.Number = number;


				post.Date = date.ToDatetimeOrDefault(DateTime.Now);
				post.CreatedAt = createdAt;
				post.LastUpdated = updatedAt;

				post.UpdatedBy = updatedBy;

				post.Year = post.Date.Year;
				post.Month = post.Date.Month;



				return post;
			}
		}

		public class MediaViewModel
		{
			

			public string contentId { get; set; }

			public int order { get; set; }

			public string type { get; set; }

			public string path { get; set; }

			public string previewPath { get; set; }

			public string name { get; set; }

			public string title { get; set; }

			public string updatedBy { get; set; }


			public int width { get; set; }

			public int height { get; set; }

			public DateTime createdAt { get; set; }

			public UploadFile MapToEntity(UploadFile entity = null)
			{
				if (entity == null) entity = new UploadFile();

				
				entity.Name = name;
				entity.Title = title;
				entity.Order = order;
				entity.Type = type;
				entity.Path = path;

				entity.Width = width;
				entity.Height = height;


				entity.PreviewPath = previewPath;

				entity.CreatedAt = createdAt;
				entity.UpdatedBy = updatedBy;
				entity.LastUpdated = createdAt;
				return entity;
			}

		}

		

		private readonly IPostService postService;

		

		public CopyController(IHostingEnvironment environment, IOptions<AppSettings> settings, IPermissionService permissionService, IPostService postService) 
			: base(environment, settings, permissionService)
		{
			this.postService = postService;
			
		}

		public IActionResult test()
		{
			return Content("test");
		}

		private string GetOleFilePath(string filePath)
		{
			string path = String.Format("Uploads/{0}", filePath);
			return Path.Combine(Settings.Value.HistoryPath, path);
		}

		private string CopyFile(string filePath)
		{
			string oleFilePath = GetOleFilePath(filePath);
			if (!System.IO.File.Exists(oleFilePath)) throw new Exception("File Not Exist: " + oleFilePath);

			string folderName = filePath.Split("/")[0];
			string fileName = filePath.Split("/")[1].ToLower();
			//檢查檔案路徑
			string folderPath = Path.Combine(this.UploadFilesPath, folderName);
			if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

			string savePath= Path.Combine(folderPath, fileName);

			System.IO.File.Copy(oleFilePath, savePath);

			return String.Format("{0}/{1}" , folderName, fileName);


		}

		//public IActionResult test()
		//{
		//	string filePath = "20180106/919bfc43-945f-4f71-b2c9-f8543d868bc5.jpg";
		//	CopyFile(filePath);


		//	return Content("done");
		//}

		IEnumerable<MediaViewModel> GetMediaViewModelsFromFile(string name, string type)
		{
			string fileName = String.Format("{0}_medias.csv", name);
			string filepath = Path.Combine(this.UploadFilesPath, fileName);

			if(!System.IO.File.Exists(filepath)) throw new Exception("File Not Found: " + filepath);

			using (var sw = new StreamReader(filepath))
			using (var reader = new CsvReader(sw))
			{
				var records = reader.GetRecords<MediaViewModel>();
				return records.ToList();

			}
		}

		IEnumerable<PostViewModel> GetPostModelsFromFile(string name, string type)
		{
			string fileName = String.Format("{0}.csv", name);
			string filepath = Path.Combine(this.UploadFilesPath, fileName);

			if (!System.IO.File.Exists(filepath)) throw new Exception("File Not Found: " + filepath);

			using (var sw = new StreamReader(filepath))
			using (var reader = new CsvReader(sw))
			{
				var records = reader.GetRecords<PostViewModel>();
				return records.ToList();

			}
		}



		public async Task<IActionResult> copy(string name, string type)
		{
			var categories = await postService.GetCategoriesAsync();
			var categoriy = categories.Where(c => c.Code == type).FirstOrDefault();

			if (categoriy == null) throw new Exception();

			var postViewModelList= GetPostModelsFromFile( name,  type);

			var mediaViewModelList = GetMediaViewModelsFromFile(name, type);

			foreach (var model in postViewModelList)
			{

				var post = model.MapToEntity();
				

				if (post.Date.Year == 3000)
				{
					//傑出校友
					var famer = categories.Where(c => c.Code == "famer").FirstOrDefault();
					post.Categories.Add(famer);
				}
				else
				{
					post.Categories.Add(categoriy);
				}

				var mediaViewModels = mediaViewModelList.Where(m => m.contentId == model.id);
				foreach (var item in mediaViewModels)
				{
					item.path = CopyFile(item.path);
					if (!String.IsNullOrEmpty(item.previewPath))
					{
						item.previewPath = CopyFile(item.previewPath);
					}
					var media = item.MapToEntity();
					post.Attachments.Add(media);
				}

				post = await postService.CreateAsync(post);

			}

			return Content("done");
		}

		
	}
}