using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.EntityFramework.DbContexts;
using ApplicationCore.Helpers;
using IdentityApp.Areas.Admin.Models;
using IdentityServer4.Models;

using ApplicationCore.Paging;
using IdentityApp.Areas.Admin.Helper;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Areas.Admin.Services;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityApp.Areas.Admin.Controllers
{
   
	public class ClientsController : BaseAdminController
	{
		private readonly IIdentityConfigService identityConfigService;

		public ClientsController(IIdentityConfigService identityConfigService)
		{
			this.identityConfigService = identityConfigService;
		}
		

			
		public async Task<IActionResult> Index(string keyword = "", int page = 1, int pageSize = 10)
		{
			var clients =await identityConfigService.FetchClients(keyword);
			
			var pageList = new PagedList<IdentityServer4.EntityFramework.Entities.Client, ClientViewModel>(clients, page, pageSize);

			foreach (var item in pageList.List)
			{
				
				pageList.ViewList.Add(ViewService.MapClientViewModel(item));
			}

			pageList.List = null;


			if (Request.IsAjaxRequest())
			{
				return Ok(pageList);
			}


			ViewData["list"] = pageList.ToJsonString();

			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			if (!Request.IsAjaxRequest()) return NotFound();


			var client = new ClientViewModel()
			{
				secret = Guid.NewGuid().ToString()
			};

			var model = new ClientEditForm
			{
				client = client,
				typeOptions = ViewService.GetClientTypeOptions(),
				
			};

			model.type = model.typeOptions.FirstOrDefault().value;

			return  Ok(model);
		}

		[HttpPost("[area]/[controller]")]
		public async Task<IActionResult> Store([FromBody] ClientEditForm model)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var client = model.client.MapToCreateEntity(model.type).ToEntity();


			client = await identityConfigService.CreateClientAsync(client);

			return Ok(client);


		}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public IActionResult Edit(int id)
		{
			var client =  identityConfigService.GetClientById(id); 
			if (client == null) return NotFound();

			var model = new ClientEditForm
			{
				client = ViewService.MapClientViewModel(client)
			};

			return  Ok(model);
		}

		[HttpPut("[area]/[controller]/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] ClientEditForm model)
		{
			var client = identityConfigService.GetClientById(id);
			if (client == null) return NotFound();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			string clientId = model.client.clientId;
			string title = model.client.title;
			string secret = model.client.secret;
			string redirectUri = model.client.redirectUri;
			string postLogoutRedirectUri = model.client.postLogoutRedirectUri;
			string frontChannelLogoutUri = model.client.frontChannelLogoutUri;

			await identityConfigService.UpdateClientAsync(client, clientId, title, secret, redirectUri, postLogoutRedirectUri, frontChannelLogoutUri);

			return Ok();


		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			await identityConfigService.DeleteClientAsync(id);

			return Ok();

		}

	}
}