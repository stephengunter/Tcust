using IdentityApp.Areas.Admin.Data;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityApp.Areas.Admin.Specifications;
using ApplicationCore.Helpers;
using System.Linq;
using IdentityApp.Areas.Admin.Helper;

namespace IdentityApp.Areas.Admin.Services
{
	public interface IIdentityConfigService
	{
		Task<IEnumerable<Client>> FetchClients(string keyword = "");
		Client GetClientById(int id);
		Task UpdateClientAsync(Client client, string clientId, string title, string secret, string uri);
	}

	public class IdentityConfigService: IIdentityConfigService
	{
		private readonly IIdentityConfigRepository<Client> clientRepository;
		private readonly IIdentityConfigRepository<ClientRedirectUri> redirectUriRepository;
		private readonly IIdentityConfigRepository<ClientPostLogoutRedirectUri> postLogoutRedirectUriConfigRepository;

		public IdentityConfigService(IIdentityConfigRepository<Client> clientRepository, 
			IIdentityConfigRepository<ClientRedirectUri> redirectUriRepository, IIdentityConfigRepository<ClientPostLogoutRedirectUri> postLogoutRedirectUriConfigRepository)
		{
			this.clientRepository = clientRepository;
			this.redirectUriRepository = redirectUriRepository;
			this.postLogoutRedirectUriConfigRepository = postLogoutRedirectUriConfigRepository;
		}

		public async Task<IEnumerable<Client>> FetchClients(string keyword = "")
		{
			Task<IEnumerable<Client>> getClientsTask;

			if (String.IsNullOrEmpty(keyword))
			{
				getClientsTask = GetAllAsync();
			}
			else
			{
				getClientsTask = GetByKeywordAsync(keyword);
			}

			var clients = await getClientsTask;

			

			return clients;



		}

		public async Task<IEnumerable<Client>> GetAllAsync()
		{

			return await clientRepository.ListAllAsync();
		}

		public async Task<IEnumerable<Client>> GetByKeywordAsync(string keyword)
		{
			var filter = new ClientFilterSpecification(keyword);

			return await clientRepository.ListAsync(filter);
		}

		public Client GetClientById(int id)
		{
			var spec = new ClientFilterSpecification(id);
			return  clientRepository.GetSingleBySpec(spec);
		}

		public async Task UpdateClientAsync(Client client, string  clientId ,string title, string secret, string uri )
		{
			if (!String.IsNullOrEmpty(clientId)) client.ClientId = clientId;

			if (!String.IsNullOrEmpty(title)) client.ClientName = title;

			if (!String.IsNullOrEmpty(uri))
			{
				client.ClientUri = uri;

				SetClientRedirectUris(client);
				SetClientPostLogoutRedirectUris(client);

			}

			if (!String.IsNullOrEmpty(secret)) SetClientSecret(client,secret);

			await clientRepository.UpdateAsync(client);
		}


		private void SetClientRedirectUris(Client client)
		{
			string redirectUri = String.Format("{0}/signin-oidc", client.ClientUri);

			if (client.RedirectUris.IsNullOrEmpty())
			{
				client.RedirectUris = new List<ClientRedirectUri>();
				client.RedirectUris.Add(new ClientRedirectUri
				{
					RedirectUri = redirectUri

				});
			}
			else
			{
				client.RedirectUris.FirstOrDefault().RedirectUri= redirectUri;
			}

		}

		private void SetClientPostLogoutRedirectUris(Client client)
		{
			string redirectUri = String.Format("{0}/signout-callback-oidc", client.ClientUri);

			if (client.PostLogoutRedirectUris.IsNullOrEmpty())
			{
				client.RedirectUris = new List<ClientRedirectUri>();
				client.PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri
				{
					PostLogoutRedirectUri = redirectUri

				});
			}
			else
			{
				client.PostLogoutRedirectUris.FirstOrDefault().PostLogoutRedirectUri = redirectUri;
			}

		}

		private void SetClientSecret(Client client, string secret)
		{
			if (client.ClientSecrets.IsNullOrEmpty())
			{
				client.ClientSecrets = new List<ClientSecret>();

				client.ClientSecrets.Add(new ClientSecret
				{
					Value = secret.Hash256(),
					Type = "SharedSecret"
				});

			}
			else
			{
				client.ClientSecrets.FirstOrDefault().Value = secret.Hash256();
			}
		}

	}
}
