using IdentityApp.Areas.Admin.Models;
using IdentityServer4.EntityFramework.Entities;
using System.Linq;
using ApplicationCore.Helpers;
using System.Collections.Generic;
using ApplicationCore.Views;
using System;

namespace IdentityApp.Areas.Admin.Helper
{
    public class ViewService
    {
		public static ClientViewModel MapClientViewModel(Client client)
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

		public static List<BaseOption> GetClientTypeOptions()
		{
			var list = new List<BaseOption>();
			foreach (string en in Enum.GetNames(typeof(ClientType)))
			{
				list.Add(new BaseOption(en, en));
			}

			return list;


		}
	}
}
