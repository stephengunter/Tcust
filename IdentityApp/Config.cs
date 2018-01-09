using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityApp
{
	public class Config
	{
		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource("apiApp","API Aplication")
			};
		}

		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile()
			};
		}

		public static List<TestUser> GetUsers()
		{
			return new List<TestUser>
			{

				new TestUser
				{
					 SubjectId="1",
					 Username="alice",
					 Password="password",

					 Claims=new List<Claim>
					 {
						  new Claim("name","Alice"),
						  new Claim("website","http://alice.com")
					 }
				},

				new TestUser
				{
					 SubjectId="2",
					 Username="bob",
					 Password="password",

					 Claims=new List<Claim>
					 {
						  new Claim("name","Bob"),
						  new Claim("website","http://bob.com")
					 }
				}
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{

				new Client
				{
					ClientId = "clientApp",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedScopes = { "apiApp" }
				},

				new Client
				{
					ClientId = "mvc",
					ClientName = "MVC Client",
					AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					
					// where to redirect to after login
					RedirectUris = { "http://localhost:50002/signin-oidc" },

					// where to redirect to after logout
					PostLogoutRedirectUris = { "http://localhost:50002/signout-callback-oidc" },

					

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"apiApp"
					},
					AllowOfflineAccess = true
				}
			};
		}

	}
}
