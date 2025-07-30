using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Server4p2
{
    public class SeedData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ConfigurationDbContext>();

            context.Database.Migrate();

            foreach (var client in Config.Clients)
            {
                context.Clients.Add(client.ToEntity());
            }
            foreach (var scope in Config.ApiScopes)
            {
                context.ApiScopes.Add(scope.ToEntity());
            }
            foreach (var resource in Config.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }
    }
}
