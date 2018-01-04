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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BlogWeb.Helpers
{
    public class ViewService
    {
		private readonly IOptions<AppSettings> settings;

		public ViewService(IOptions<AppSettings> settings)
		{
			this.settings = settings;
		}

		public PostViewModel MapPostViewModel(Post post, bool allMedias = false)
		{
			var model = new PostViewModel();
			model.id = post.Id;
			model.title = post.Title;
			model.author = post.Author;
			model.content = post.Content;
			model.date = post.Date.ToShortDateString();
			model.createdAt = post.CreatedAt;

			if (String.IsNullOrEmpty(post.Summary)) model.summary = PostViewModel.GetDefaultSummary(post);
			else model.summary = post.Summary;

			if (allMedias)
			{
				model.medias = new List<MediaViewModel>();
				foreach (var media in post.Attachments)
				{

					model.medias.Add(MapMediaViewModel(media));
				}
			}
			else
			{
				if (post.TopFile != null) model.cover = MapMediaViewModel(post.TopFile);
			}

			

			return model;
		}

		public MediaViewModel MapMediaViewModel(UploadFile file)
		{
			var model = new MediaViewModel();

			model.id = file.Id;
			model.postId = file.PostId;
			model.name = file.Name;
			model.title = file.Title;
			model.order = file.Order;

			model.width = file.Width;
			model.height = file.Height;

			model.path = String.Format("{0}/{1}/{2}", settings.Value.Url , settings.Value.UploadFoler, file.Path); 

			return model;
		}

		



	}
}
