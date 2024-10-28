using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Security.Claims;
using TicketingSystem.DataAccess;
using TicketingSystem.Handler;


var builder = WebApplication.CreateBuilder(args);
//Context
builder.Services.AddDbContext<AppContextDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//EndraID
// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Autentificare Azure AD și configurarea evenimentelor pentru OpenID Connect
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
//    .EnableTokenAcquisitionToCallDownstreamApi()
//    .AddInMemoryTokenCaches();

// Adaugă evenimente pentru OpenID Connect
builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<TicketingSystem.Handler.UserService>();

            // Obținem Azure ID-ul și numele utilizatorului conectat
            var azureId = context.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            var name = context.Principal.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(azureId) && !string.IsNullOrEmpty(name))
            {
                // Înregistrăm sau actualizăm utilizatorul
                await userService.RegisterOrUpdateUserAsync(azureId, name);

                // Adăugăm datele utilizatorului în claims pentru acces ulterior
                var claims = new List<Claim>
                {
                    new Claim("AzureID", azureId),
                    new Claim("Name", name)
                };

                var appIdentity = new ClaimsIdentity(claims);
                context.Principal.AddIdentity(appIdentity);
            }
        }
    };
    options.Events.OnSignedOutCallbackRedirect = async context =>
    {
        context.Response.Redirect("/Home/Index");  // Redirecționează la Home după sign out
        context.HandleResponse();
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CurrentUserService>();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
