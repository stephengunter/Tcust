using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using ApplicationCore.Views;
using IdentityServer4;

namespace IdentityApp.Areas.Admin.Models
{

	public enum ClientType
	{
		MVC,
		Javascript,
		Console

	}


	public class ClientViewModel
    {
		public int id { get; set; }

		[Required(ErrorMessage = "請填寫ClientId")]
		public string clientId { get; set; }

		public string title { get; set; }
		public string secret { get; set; }

		
		public string uri { get; set; }

		public string redirectUri { get; set; }


		public string postLogoutRedirectUri { get; set; }

		public string frontChannelLogoutUri { get; set; }



		public Client MapToCreateEntity(string type)
		{
			var clientType = (ClientType)Enum.Parse(typeof(ClientType), type);

			var client = new Client() { ClientSecrets = new List<Secret>() };
			client.RequireConsent = false;

			client.ClientId = clientId;
			client.ClientName =title;
			client.ClientUri = uri;

			

			if (String.IsNullOrEmpty(secret)) secret = "secret";
			
			client.ClientSecrets.Add(new Secret(secret.Sha256()));

			if (clientType == ClientType.MVC)
			{
				client.AllowedGrantTypes = GrantTypes.HybridAndClientCredentials;
				client.AllowOfflineAccess = true;
				client.AlwaysSendClientClaims = true;
				client.AlwaysIncludeUserClaimsInIdToken = true;

				client.AllowedScopes = new List<string>()
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					"apiApp"
				};

				client.RedirectUris = new List<string>()
				{
					String.Format("{0}/signin-oidc",client.ClientUri)

				};
				client.PostLogoutRedirectUris = new List<string>()
				{
					String.Format("{0}/signout-callback-oidc",client.ClientUri)

				};

				client.FrontChannelLogoutUri = String.Format("{0}/account/logout", client.ClientUri);

			}
			else if (clientType == ClientType.Javascript)
			{

				client.AllowedGrantTypes = GrantTypes.Implicit;
				client.AllowAccessTokensViaBrowser = true;



				client.RedirectUris = new List<string>()
				{
					String.Format("{0}/callback.html",client.ClientUri)

				};

				client.PostLogoutRedirectUris = new List<string>()
				{
					String.Format("{0}/index.html",client.ClientUri)

				};

				client.AllowedCorsOrigins = new List<string>()
				{
					client.ClientUri

				};


				client.AllowedScopes = new List<string>()
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					"apiApp"
				};

			}
			else
			{
				client.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
				client.AllowedScopes = new List<string>()
				{
					"apiApp"
				};

			}


			return client;
		}
	}

	public class ClientEditForm
	{
		public string type { get; set; }

		public ClientViewModel client { get; set; }

		public List<BaseOption> typeOptions { get; set; }
	}
}
