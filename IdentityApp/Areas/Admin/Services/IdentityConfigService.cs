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
		Task<Client> CreateClientAsync(Client client);
		Task UpdateClientAsync(Client client, string clientId, string title, string secret, string redirectUri, string postLogoutRedirectUri,string frontChannelLogoutUri);
		Task DeleteClientAsync(int id);

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
			
			return await clientRepository.ListAsync(new BaseClientFilterSpecification());
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

		public async Task<Client> CreateClientAsync(Client client)
		{
			return await clientRepository.AddAsync(client);
		}

		public async Task UpdateClientAsync(Client client, string  clientId ,string title, string secret, string redirectUri, string postLogoutRedirectUri, string frontChannelLogoutUri)
		{
			if (!String.IsNullOrEmpty(clientId)) client.ClientId = clientId;

			if (!String.IsNullOrEmpty(title)) client.ClientName = title;

			if (String.IsNullOrEmpty(redirectUri))
			{
				if (!client.RedirectUris.IsNullOrEmpty())
				{
					redirectUriRepository.DbSet.RemoveRange(client.RedirectUris);
				}
			}
			else
			{
				SetClientRedirectUris(client, redirectUri);
			}

			if (String.IsNullOrEmpty(postLogoutRedirectUri))
			{
				if (!client.PostLogoutRedirectUris.IsNullOrEmpty())
				{
					postLogoutRedirectUriConfigRepository.DbSet.RemoveRange(client.PostLogoutRedirectUris);
				}
			}
			else
			{
				SetClientPostLogoutRedirectUris(client, postLogoutRedirectUri);
			}
			

			if (!String.IsNullOrEmpty(secret)) SetClientSecret(client,secret);

			client.FrontChannelLogoutUri = frontChannelLogoutUri;

			await clientRepository.UpdateAsync(client);
		}

		public async Task DeleteClientAsync(int id)
		{
			var entity =await clientRepository.GetByIdAsync(id);
			await clientRepository.DeleteAsync(entity);
		}


		private void SetClientRedirectUris(Client client,string redirectUri)
		{
			
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

		private void SetClientPostLogoutRedirectUris(Client client, string postLogoutRedirectUri)
		{
			
			if (client.PostLogoutRedirectUris.IsNullOrEmpty())
			{
				client.PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>();
				client.PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri
				{
					PostLogoutRedirectUri = postLogoutRedirectUri

				});
			}
			else
			{
				client.PostLogoutRedirectUris.FirstOrDefault().PostLogoutRedirectUri = postLogoutRedirectUri;
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
