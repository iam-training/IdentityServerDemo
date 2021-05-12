using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerDemo
{
    internal static class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Client Credentials Flow
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Client Credentials Flow Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "api1.read" },
                    Enabled = true
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "api1",
                    DisplayName = "API 1",
                    Description = "API 1 Read access",
                    Scopes = { "api1.read" },
                    ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read", "Read API1")
            };
        }

    }
}
