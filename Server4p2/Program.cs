using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Server4p2.Models;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Postgres");
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

builder.Services
    .AddDefaultIdentity<ApplicationUser>(o =>
    {
        o.SignIn.RequireConfirmedAccount = false;  
    })
    .AddRoles<IdentityRole>()                   
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

builder.Services
    .AddIdentityServer(o =>
    {
        o.Authentication.CookieSameSiteMode = SameSiteMode.Lax;
    })
    .AddAspNetIdentity<ApplicationUser>()            
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = b =>
            b.UseNpgsql(conn, npg => npg.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = b =>
            b.UseNpgsql(conn, npg => npg.MigrationsAssembly(migrationsAssembly));
        opt.EnableTokenCleanup = true;
    })
    .AddDeveloperSigningCredential();

builder.Services.AddTransient<IProfileService, ProfileService>();

builder.Services.PostConfigure<CookieAuthenticationOptions>(
    IdentityConstants.ApplicationScheme,
    o =>
    {
        o.Cookie.SameSite = SameSiteMode.Lax;
        o.Cookie.SecurePolicy = CookieSecurePolicy.None;
    });

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   
app.UseIdentityServer();   
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//using (var scope = app.Services.CreateScope())
//{
//    var cfg = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
//    if (!cfg.IdentityResources.Any(r => r.Name == "email"))
//    {
//        cfg.IdentityResources.Add(new IdentityServer4.EntityFramework.Entities.IdentityResource
//        {
//            Name = "email",
//            DisplayName = "Email",
//            Enabled = true,
//            ShowInDiscoveryDocument = true,
//            UserClaims = { new() { Type = "email" } }
//        });
//        cfg.SaveChanges();
//    }
//}

//using (var scope = app.Services.CreateScope())
//{
//    var cfg = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
//    if (cfg == null)
//    {
//        Console.WriteLine("ConfigurationDbContext is null");
//        return;
//    }

//    try
//    {
//// IdentityResources
//if (!cfg.IdentityResources.Any())
//{
//    cfg.IdentityResources.AddRange(
//        new IdentityServer4.EntityFramework.Entities.IdentityResource
//        {
//            Name = "openid",
//            DisplayName = "OpenID",
//            Enabled = true,
//            Required = true,
//            Emphasize = false,
//            ShowInDiscoveryDocument = true,
//            UserClaims = new List<IdentityServer4.EntityFramework.Entities.IdentityResourceClaim>
//            {
//        new() { Type = "sub" }
//            }
//        },
//        new IdentityServer4.EntityFramework.Entities.IdentityResource
//        {
//            Name = "profile",
//            DisplayName = "User profile",
//            Enabled = true,
//            UserClaims = new List<IdentityServer4.EntityFramework.Entities.IdentityResourceClaim>
//            {
//        new() { Type = "name" },
//        new() { Type = "family_name" },
//        new() { Type = "given_name" },
//        new() { Type = "preferred_username" }
//            }
//        });
//    cfg.SaveChanges();
//}

// ApiScopes
//if (!cfg.ApiScopes.Any())
//{
//    cfg.ApiScopes.Add(new IdentityServer4.EntityFramework.Entities.ApiScope
//    {
//        Name = "api1",
//        DisplayName = "Demo API",
//        Enabled = true,
//        ShowInDiscoveryDocument = true
//    });
//    cfg.SaveChanges();
//}

//Clients

//        var client = new IdentityServer4.EntityFramework.Entities.Client
//        {
//           ClientId = "client2",
//           ClientName = "Test Client2",
//           ProtocolType = "oidc",
//           RequirePkce = true,
//           RequireClientSecret = true,

//            AllowedGrantTypes = new List<IdentityServer4.EntityFramework.Entities.ClientGrantType>
//        {
//            new() { GrantType = "authorization_code" }
//        },

//            ClientSecrets = new List<IdentityServer4.EntityFramework.Entities.ClientSecret>
//        {
//            new() { Value = "secret".Sha256(), Type = "SharedSecret" }
//        },

//            RedirectUris = new List<IdentityServer4.EntityFramework.Entities.ClientRedirectUri>
//        {
//            new() { RedirectUri = "http://localhost:5181/signin-oidc" }
//        },

//            PostLogoutRedirectUris = new List<IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri>
//        {
//            new() { PostLogoutRedirectUri = "http://localhost:5181/signout-callback-oidc" }
//        },

//            AllowedScopes = new List<IdentityServer4.EntityFramework.Entities.ClientScope>
//        {
//            new() { Scope = "openid"  },
//            new() { Scope = "profile" },
//            new() { Scope = "api1"    }
//        }
//        };
//        if (!cfg.Clients.Any(c => c.ClientId == "client2"))
//        {
//            cfg.Clients.Add(client);
//            cfg.SaveChanges();
//        }


//        cfg.SaveChanges();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error initializing data: {ex.Message}");
//    }
//}

// Configure the HTTP request pipeline.
app.Run();