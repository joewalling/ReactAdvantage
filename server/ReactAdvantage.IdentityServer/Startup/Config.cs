using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using ReactAdvantage.IdentityServer.Startup;

namespace ReactAdvantage.IdentityServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("ReactAdvantageApi", "ReactAdvantage API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                // GraphQL Playground Client
                new Client
                {
                    ClientId = "graphqlPlaygroundJs",
                    ClientName = "GraphQL Playground Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { "https://localhost:44398/Home/GraphqlPlaygroundAuthCallback" },
                    PostLogoutRedirectUris = { "https://localhost:44398/Home/GraphqlPlayground" },
                    AllowedCorsOrigins =     { "https://localhost:44398" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ReactAdvantageApi"
                    }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return TestUsers.Users;
        }
    }
}
