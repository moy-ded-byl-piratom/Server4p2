using IdentityServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Domain.Dtos;
using IdentityServer.DataAccess.Repositories;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;

namespace IdentityServer.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task AddClientAsync(ClientDto clientDto)
        {
            var client = new IdentityServer4.EntityFramework.Entities.Client
            {
                ClientId = clientDto.ClientId,
                ClientName = clientDto.ClientName,
                ProtocolType = "oidc",
                RequirePkce = clientDto.RequirePkce,
                RequireClientSecret = clientDto.RequireClientSecret,
                AllowedGrantTypes = new List<ClientGrantType>
                {
                    new ClientGrantType { GrantType = "authorization_code" }
                },
                ClientSecrets = new List<ClientSecret>
                {
                    new ClientSecret { Value = clientDto.ClientSecret.Sha256(), Type = "SharedSecret" }
                },
                RedirectUris = clientDto.RedirectUris.Select(uri => new ClientRedirectUri { RedirectUri = uri }).ToList(),
                PostLogoutRedirectUris = clientDto.PostLogoutRedirectUris.Select(uri => new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = uri }).ToList(),
                AllowedScopes = clientDto.AllowedScopes.Select(scope => new ClientScope { Scope = scope }).ToList(),
                AllowOfflineAccess = clientDto.AllowOfflineAccess,
                AccessTokenLifetime = clientDto.AccessTokenLifetime
            };

            await _clientRepository.AddClientAsync(client);
        }
    }
}
