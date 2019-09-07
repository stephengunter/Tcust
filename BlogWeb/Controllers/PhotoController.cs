using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Blog.Models;
using Blog.Services;
using ApplicationCore.Helpers;
using Blog.Views;
using ApplicationCore.Paging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Blog.Helpers;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace BlogWeb.Controllers
{
	public class PhotoController : BaseController
	{
		public PhotoController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
		{
		}

		public IActionResult Index(int width, int height, string type, string path)
		{
			//檢查檔案路徑
			string imgSourcePath = Path.Combine(this.UploadFilesPath, path);
			if (!System.IO.File.Exists(imgSourcePath)) throw new Exception(String.Format("圖片路徑無效:{0}", imgSourcePath));

		
			string extension = (System.IO.Path.HasExtension(imgSourcePath)) ?
											  System.IO.Path.GetExtension(imgSourcePath).Substring(1).ToLower() :
											  string.Empty;
			if (!("jpg".Equals(extension) || "gif".Equals(extension) || "png".Equals(extension)))
			{
				throw new Exception(String.Format("圖片格式錯誤:{0}", imgSourcePath));
			}

			// 長寬數值不正確時, 回傳原圖
			if (width <= 0 || height <= 0)
			{
				return SendOriginalImage(imgSourcePath);
				
			}

			Image imgSource = Image.FromStream(new MemoryStream(System.IO.File.ReadAllBytes(imgSourcePath)));
			Image imgResized = null;
			if (type == "crop") imgResized = GetCropedImage(imgSource, width, height);
			else imgResized = GetResizedImage(imgSource, width, height);

			if (imgResized == null)
			{
				return SendOriginalImage(imgSourcePath);
			}
			else
			{
				Stream outputStream = new MemoryStream();

				imgResized.Save(outputStream, ImageFormat.Jpeg);
				outputStream.Seek(0, SeekOrigin.Begin);

				return this.File(outputStream, "image/jpeg");
			}
		}

		
		// 傳回原始圖片
		private IActionResult SendOriginalImage(string imgSourcePath)
		{
			string type = "image/jpeg";

			string ext = Path.GetExtension(imgSourcePath).ToLower();
			if(ext== "png") type= "image/png";
			else if (ext == "gif") type = "image/gif";

			var image = System.IO.File.OpenRead(imgSourcePath);
			return File(image, type);
			
		}


		private Image GetResizedImage(Image imgSource, int width, int height)
		{
			// 依比例縮圖
			// 計算大小
			if (width <= imgSource.Width && height >= (1.0 * imgSource.Height * width / imgSource.Width))
			{
				height = (int)(1.0 * imgSource.Height * width / imgSource.Width);
			}
			else if (height <= imgSource.Height && width >= (1.0 * imgSource.Width * height / imgSource.Height))
			{
				width = (int)(1.0 * imgSource.Width * height / imgSource.Height);
			}

			// 如果需要縮圖
			if (width < imgSource.Width || height < imgSource.Height)
			{
				return ImageResizer.ScaleByFixedSize(imgSource, width, height);
			}

			return null;
			
			
		}

		private Image GetCropedImage(Image imgSource,int width, int height)
		{
			
			bool isSizeChanged = false;
			// 計算截圖範圍
			int cropWidth = imgSource.Width;
			int cropHeight = imgSource.Height;

			if (width < imgSource.Width && height < imgSource.Height)
			{
				if (1.0 * imgSource.Width / width * height <= imgSource.Height)
				{
					cropHeight = (int)(1.0 * imgSource.Width / width * height);
					isSizeChanged = true;
				}
				else if (1.0 * imgSource.Height / height * width <= imgSource.Width)
				{
					cropWidth = (int)(1.0 * imgSource.Height / height * width);
					isSizeChanged = true;
				}
			}
			else if (width < imgSource.Width && height >= imgSource.Height)
			{
				cropWidth = (int)(1.0 * imgSource.Height / height * width);
				isSizeChanged = true;
			}
			else if (width >= imgSource.Width && height < imgSource.Height)
			{
				cropHeight = (int)(1.0 * imgSource.Width / width * height);
				isSizeChanged = true;
			}
			else
			{
				// 原圖
			}

			// 進行截圖及縮圖
			if (isSizeChanged)
			{
				var cropedImage = ImageResizer.Crop(imgSource, cropWidth, cropHeight, AnchorPosition.Center);
				return ImageResizer.ScaleByFixedSize(cropedImage, width, height);
			}


			return null;
			
			
		}
	}
}