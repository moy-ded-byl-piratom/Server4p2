using IdentityServer.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Domain.Interfaces
{
    public interface IClientService
    {
        Task AddClientAsync(ClientDto clientDto);
    }
}
