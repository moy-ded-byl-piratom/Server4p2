using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityServer.Domain.Models.IdentityServer.Domain;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer.Domain.Models
{
    public class ProfileService : IProfileService
    {
        readonly UserManager<IdentityServer.Domain.ApplicationUser> _users;
        public ProfileService(UserManager<ApplicationUser> users) => _users = users;

        public async Task GetProfileDataAsync(ProfileDataRequestContext ctx)
        {
            var user = await _users.GetUserAsync(ctx.Subject);
            ctx.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
        }

        public async Task IsActiveAsync(IsActiveContext ctx)
        {
            var user = await _users.GetUserAsync(ctx.Subject);
            ctx.IsActive = user != null;
        }
    }
}
