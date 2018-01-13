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
					ClientId = "blog-mvc",
					ClientName = "Blog MVC",
					AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

					RequireConsent = false,
					AllowOfflineAccess = true,
					AlwaysSendClientClaims=true,
					AlwaysIncludeUserClaimsInIdToken=true,

					

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
					

				},
				// JavaScript Client
				new Client
				{
					ClientId = "js",
					ClientName = "JavaScript Client",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,

					RedirectUris =           { "http://localhost:50003/callback.html" },
					PostLogoutRedirectUris = { "http://localhost:50003/index.html" },
					AllowedCorsOrigins =     { "http://localhost:50003" },

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"apiApp"
					}
				}



			};
		}

	}
}
