using IdentityApp.Areas.Admin.Models;
using IdentityServer4.EntityFramework.Entities;
using System.Linq;
using ApplicationCore.Helpers;

namespace IdentityApp.Areas.Admin.Helper
{
    public class ViewService
    {
		public ClientViewModel MapClientViewModel(Client client)
		{
			var model = new ClientViewModel();
			model.id = client.Id;
			
			model.title = client.ClientName;
			model.clientId = client.ClientId;
			model.uri = client.ClientUri;
			model.frontChannelLogoutUri = client.FrontChannelLogoutUri;

			if (!client.RedirectUris.IsNullOrEmpty())
			{
				model.redirectUri = client.RedirectUris.FirstOrDefault().RedirectUri;
			}

			if (!client.PostLogoutRedirectUris.IsNullOrEmpty())
			{
				model.postLogoutRedirectUri = client.PostLogoutRedirectUris.FirstOrDefault().PostLogoutRedirectUri;
			} 


			return model;
		}
	}
}
