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

namespace IdentityApp.Areas.Admin.Controllers
{
   
	public class ClientsController : BaseAdminController
	{
		private readonly IIdentityConfigService identityConfigService;
		private readonly ViewService viewService;

		public ClientsController(IIdentityConfigService identityConfigService)
		{
			this.identityConfigService = identityConfigService;

			this.viewService=new ViewService();
		}
		

			
		public async Task<IActionResult> Index(string keyword = "", int page = 1, int pageSize = 10)
		{
			var clients =await identityConfigService.FetchClients(keyword);
			
			var pageList = new PagedList<IdentityServer4.EntityFramework.Entities.Client, ClientViewModel>(clients, page, pageSize);

			foreach (var item in pageList.List)
			{
				
				pageList.ViewList.Add(viewService.MapClientViewModel(item));
			}

			pageList.List = null;


			if (Request.IsAjaxRequest())
			{
				return new ObjectResult(pageList);
			}


			ViewData["list"] = pageList.ToJsonString();

			return View();
		}

		//[HttpGet]
		//public IActionResult Create()
		//{
		//	if (!Request.IsAjaxRequest()) return NotFound();


		//	var client = new ClientViewModel()
		//	{
		//		 clientId = Guid.NewGuid().ToString(),
		//		 secret = Guid.NewGuid().ToString()
		//	};
			
		//	var model = new ClientEditForm
		//	{
		//		client = client
		//	};

		//	return new ObjectResult(model);
		//}

		//[HttpPost("[area]/[controller]")]
		//public async Task<IActionResult> Store([FromBody] ClientEditForm model)
		//{

		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	var client = model.client.MapToEntity();

		//	var secret = new Secret(model.client.secret.Sha256());
		//	client.ClientSecrets.Add(secret) ;

		//	client.AllowedGrantTypes = GrantTypes.HybridAndClientCredentials;
			

		//	client.RequireConsent = false;
		//	client.AllowOfflineAccess = true;
		//	client.AlwaysSendClientClaims = true;
		//	client.AlwaysIncludeUserClaimsInIdToken = true;

		//	client.AllowedScopes = new List<string>()
		//	{
		//		IdentityServerConstants.StandardScopes.OpenId,
		//		IdentityServerConstants.StandardScopes.Profile
		//	};

		//	client.RedirectUris = new List<string>()
		//	{
		//		String.Format("{0}/signin-oidc",client.ClientUri)
				
		//	};
		//	client.PostLogoutRedirectUris = new List<string>()
		//	{
		//		String.Format("{0}/signout-callback-oidc",client.ClientUri)

		//	};

			
		//	await configurationDbContext.Clients.AddAsync(client.ToEntity());
		//	await configurationDbContext.SaveChangesAsync();

		//	return new ObjectResult(client);


		//}

		[HttpGet("[area]/[controller]/{id}/edit")]
		public IActionResult Edit(int id)
		{
			var client =  identityConfigService.GetClientById(id); 
			if (client == null) return NotFound();

			var model = new ClientEditForm
			{
				client = viewService.MapClientViewModel(client)
			};

			return new ObjectResult(model);
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
			string uri = model.client.uri;

			await identityConfigService.UpdateClientAsync(client, clientId, title, secret,  uri);

			return new NoContentResult();


		}

		

	}
}