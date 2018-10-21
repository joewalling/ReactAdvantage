using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using ReactAdvantage.Domain.Configuration;

namespace ReactAdvantage.IdentityServer.Startup
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(ApiResources.ReactAdvantageApi, "ReactAdvantage API", new[] { JwtClaimTypes.Subject, JwtClaimTypes.Email, JwtClaimTypes.Role, ApplicationClaimTypes.TenantId })
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(BaseUrls baseUrls, IHostingEnvironment environment)
        {
            
            var clients = new List<Client>
            {
                // GraphQL Playground Client
                new Client
                {
                    ClientId = "graphqlPlaygroundJs",
                    ClientName = "GraphQL Playground Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { $"{baseUrls.GraphqlPlaygroundJsClient}/Home/GraphqlPlaygroundAuthCallback" },
                    PostLogoutRedirectUris = { $"{baseUrls.GraphqlPlaygroundJsClient}/Home/GraphqlPlayground" },
                    AllowedCorsOrigins =     { baseUrls.GraphqlPlaygroundJsClient },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ApiResources.ReactAdvantageApi
                    }
                },

                // React Client
                new Client
                {
                    ClientId = "react",
                    ClientName = "React Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { $"{baseUrls.ReactClient}/authentication/callback", $"{baseUrls.ReactClient}/authentication/silentCallback" },
                    PostLogoutRedirectUris = { $"{baseUrls.ReactClient}/" },
                    AllowedCorsOrigins =     { baseUrls.ReactClient },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ApiResources.ReactAdvantageApi
                    }
                }
            };

            if (!string.IsNullOrEmpty(baseUrls.ReactClientLocal))
            {
                // Additional React Client for local development, if different from regular react client
                clients.Add(new Client
                {
                    ClientId = "reactLocal",
                    ClientName = "React Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = { $"{baseUrls.ReactClientLocal}/authentication/callback", $"{baseUrls.ReactClientLocal}/authentication/silentCallback" },
                    PostLogoutRedirectUris = { $"{baseUrls.ReactClientLocal}/" },
                    AllowedCorsOrigins = { baseUrls.ReactClientLocal },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ApiResources.ReactAdvantageApi
                    }
                });
            }

            if (environment.IsEnvironment("Test"))
            {
                //Client for Integration Tests
                clients.Add(new Client
                {
                    ClientId = "testClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ApiResources.ReactAdvantageApi
                    }
                });
            }

            return clients;
        }
    }
}
