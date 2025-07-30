
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.DataAccess.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ConfigurationDbContext _context;

        public ClientRepository(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task AddClientAsync(Client client)
        {
            if (!await _context.Clients.AnyAsync(c => c.ClientId == client.ClientId))
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
        }
    }

    public interface IClientRepository
    {
        Task AddClientAsync(Client client);
    }
}
