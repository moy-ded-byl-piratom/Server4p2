using IdentityServer4.Models;
using static System.Net.Mime.MediaTypeNames;

namespace IdentityServer.Domain.Models
{
    public class ClientConfig
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "openid", "profile", "email", "api1" }
                },

                new Client
                {
                ClientId = "client2",
                ClientName = "Test Client2",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RequirePkce = true,
                RequireClientSecret = true,
                AllowedScopes = { "openid", "profile", "email", "api1" },
                ClientSecrets = { new Secret("secret".Sha256()) },

                }
            };
        }
    }
}
