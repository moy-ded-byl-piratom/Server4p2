using IdentityServer.DataAccess;
using IdentityServer.Domain.Models;
using IdentityServer.Domain.Models.IdentityServer.Domain;
using IdentityServer.Infrastructure.Logging;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var conn = builder.Configuration.GetConnectionString("Postgres");
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(conn));

builder.Services.AddSerilog();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(o =>
    {
        o.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
    //.AddInMemoryClients(ClientConfig.GetClients())
    //.AddInMemoryApiScopes(ApiScopeConfig.GetApiScopes())
    //.AddInMemoryApiResources(ApiResourceConfig.GetApiResources())
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

builder.Services.PostConfigure<CookieAuthenticationOptions>(
    IdentityConstants.ApplicationScheme,
    o =>
    {
        o.Cookie.SameSite = SameSiteMode.Lax;
        o.Cookie.SecurePolicy = CookieSecurePolicy.None;
    });

// Register the ProfileService
builder.Services.AddTransient<IProfileService, ProfileService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
