using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Server4p2
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "nickname",
                    DisplayName = "Nickname",
                    UserClaims = { "nickname" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
           new List<ApiScope>
           {
                new ApiScope("api1", "My API")
           };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "mvc_client",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = { "https://localhost:5225/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5225/signout-callback-oidc" },
                    AllowedScopes = { "openid", "profile", "nickname", "api1" },
                    AllowOfflineAccess = true
                }
            };
    }
}
