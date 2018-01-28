using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using History.DAL.Tccn;
using Microsoft.EntityFrameworkCore;
using System.IO;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;

namespace HistoryApp.Areas.Admin.Controllers
{
	public class CopyController : BaseAdminController
	{
		string DAIRY_ID = "7f266b34-c03e-41ae-84a1-1e27dccc7039"; //校園日誌
		string HONOR_ID = "4af854f1-d569-4394-9a89-3ac1383734c0"; //榮譽榜
		string NEWS_ID = "5ccee554-0fc6-4752-bc58-3bf2b2f8cc97"; //大愛新聞

		private readonly IHostingEnvironment hostingEnvironment;
		private readonly TccnContext tccnContext;

		private readonly string exportPath;

		public CopyController(IHostingEnvironment hostingEnvironment,TccnContext tccnContext)
		{
			this.hostingEnvironment = hostingEnvironment;
			this.tccnContext = tccnContext;

			this.exportPath = Path.Combine(hostingEnvironment.WebRootPath,"export");
		}

		IQueryable<Content> Contents
		{
			get { return tccnContext.Contents.Include("FileUplaods"); }
		}

		

		public IActionResult Index()
        {
			var test = GetDiaryList().Count();
			return Content(test.ToString());



		}

		public ActionResult Export(int count = 0)
		{
			
			string category = "diary";
			var dairyList = GetDiaryList().Where(c => c.ContentCreateTime.HasValue
							&& Convert.ToDateTime(c.ContentCreateTime).Year >= 2013);



			if (count > 0) exportFile(category, dairyList.Take(count).ToList());
			else exportFile(category, dairyList.ToList());




			category = "honor";
			var honorList = GetHonorList().Where(c => c.ContentCreateTime.HasValue
							&& Convert.ToDateTime(c.ContentCreateTime).Year >= 2013);



			if (count > 0) exportFile(category, honorList.Take(count).ToList());
			else exportFile(category, honorList.ToList());


			category = "famer";
			var famerList = GetFamerList();


			if (count > 0) exportFile(category, famerList.Take(count).ToList());
			else exportFile(category, famerList.ToList());


			category = "da-ai";
			var newsList = GetNewsList();


			if (count > 0) exportFile(category, newsList.Take(count).ToList());
			else exportFile(category, newsList.ToList());



			return Content("done");

		}

		private void exportFile(string category, IEnumerable<Content> contents)
		{
			string date = DateTime.Now.ToString("yyyyMMdd");
			string fileName = String.Format("{0}{1}.csv", date, category);
			string filepath = Path.Combine(this.exportPath, fileName);  


			var posts = new List<PostViewModel>();
			var attachments = new List<MediaViewModel>();
			foreach (var item in contents)
			{
				var model = new PostViewModel(item);
				model.termNumber = GetTermNumber(item);
				model.categoryName = category;

				posts.Add(model);


				foreach (var uploadFile in item.FileUplaods)
				{
					var mediaModel = new MediaViewModel(uploadFile);
					attachments.Add(mediaModel);
				}
			}

			WritePosts(filepath, posts);

		

			fileName = String.Format("{0}{1}_medias.csv", date, category);
			filepath = Path.Combine(this.exportPath, fileName);
			WriteAttachments(filepath, attachments);
		}

		private void WritePosts(string filepath, List<PostViewModel> posts)
		{
			using (var sw = new StreamWriter(filepath))
			using (var writer = new CsvWriter(sw))
			{
				writer.WriteRecords(posts);
			}
		}

		private void WriteAttachments(string filepath, List<MediaViewModel> attachments)
		{
			using (var sw = new StreamWriter(filepath))
			using (var writer = new CsvWriter(sw))
			{
				writer.WriteRecords(attachments);
			}
		}

		private IEnumerable<Content> GetDiaryList()
		{
			var ids = tccnContext.ContentMultipleTypes.Where(c => c.ContentType == DAIRY_ID)
													  .Select(c => c.ContentID).ToList();

			return Contents.Where(c => ids.Contains(c.ContentID));
		}

		private IEnumerable<Content> GetFamerList()
		{
			var date = new DateTime(2055, 1, 1);
			return Contents.Where(c => c.ContentTime.HasValue && c.ContentTime > date);
		}

		IList<string> HonorListIds()
		{
			var ids = tccnContext.ContentMultipleTypes.Where(c => c.ContentType == HONOR_ID)
													  .Select(c => c.ContentID).ToList();

			return ids;
		}

		private IEnumerable<Content> GetHonorList()
		{
			var ids = HonorListIds();
			return Contents.Where(c => ids.Contains(c.ContentID));

		}


		//大愛
		private IEnumerable<Content> GetNewsList()
		{
			return Contents.Where(c => c.TypeID == NEWS_ID);
		}
		private string[] GetSchoolYearTerm(Content content)
		{
			string year = "";

			var educated = tccnContext.Educateds.Find(content.ContentID);

			var categoryYear = tccnContext.Categories.Find(educated.CategoryYearID);
			if (categoryYear != null) year = categoryYear.CategoryName;

			string term = "";
			var categoryTerm = tccnContext.Categories.Find(educated.CategoryTermID);
			if (categoryTerm != null) term = categoryTerm.CategoryName;

			return new string[] { year, term };
		}
		private string GetTermNumber(Content content)
		{
			var arr = GetSchoolYearTerm(content);
			int number = 0;
			if (arr[1].Contains("一") || arr[1].Contains("1")) number = 1;
			else if (arr[1].Contains("二") || arr[1].Contains("2")) number = 2;
			if (arr[1].Contains("三") || arr[1].Contains("3")) number = 3;
			if (arr[1].Contains("四") || arr[1].Contains("4")) number = 4;

			return arr[0] + number.ToString();
		}
	}


	public class PostViewModel
	{
		public PostViewModel()
		{

		}
		public PostViewModel(Content content)
		{
			id = content.ContentID;
			title = content.ContentName;
			number = content.SerialNo;
			author = content.Author;
			this.content = content.ContentText;

			this.createdAt = Convert.ToDateTime(content.ContentCreateTime);
			this.updatedAt = Convert.ToDateTime(content.ContentUpdateTime);

			updatedBy = content.ContentUpdater;

			if (String.IsNullOrEmpty(updatedBy)) throw new Exception("updatedBy==null");

			if (content.ContentTime.HasValue)
			{
				date = Convert.ToDateTime(content.ContentTime).ToShortDateString();
			}
			else
			{
				date = this.createdAt.ToShortDateString();
			}


			fileIds = string.Join(",", content.FileUplaods.Select(f => f.ItemOID));


		}

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
	}

	public class MediaViewModel
	{
		public MediaViewModel() { }

		public MediaViewModel(FileUplaod file)
		{

			this.contentId = file.ContentID;
			this.type = file.FunctionType.ToLower();

			this.name = file.FileName;
			this.title = file.FileName.Split(new char[] { '.' })[0];

			this.updatedBy = file.Creator;


			this.order = file.CoverImage ? 1 : 0;

			this.width = file.ImageWidth.HasValue ? (int)file.ImageWidth : 0;
			this.height = file.ImageHeight.HasValue ? (int)file.ImageHeight : 0;

			this.path = file.Path;
			this.previewPath = file.PreviewPath;

			this.createdAt = (DateTime)file.CreateTime;
		}



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

	}
}