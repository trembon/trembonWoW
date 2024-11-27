using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using MySqlConnector;
using trembonWoW.Authentication;
using trembonWoW.Client.Pages;
using trembonWoW.Components;
using trembonWoW.Core.Connectors.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("AuthDB")!);
builder.Services.AddTransient<IAuthDatabase, AuthDatabase>();

builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

builder.Services.AddIdentityCore<WoWUser>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IUserStore<WoWUser>, WoWUserStore>();
builder.Services.AddSingleton<IPasswordHasher<WoWUser>, WoWPasswordHasher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(trembonWoW.Client._Imports).Assembly);

app.Run();