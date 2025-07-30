using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Domain.Dtos
{
    public class ClientDto
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public bool RequirePkce { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ClientSecret { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public List<string> AllowedScopes { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public int AccessTokenLifetime { get; set; }
    }
}