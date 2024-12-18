using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using MySqlConnector;
using System.Text;
using trembonWoW;
using trembonWoW.Core.Authentication;
using trembonWoW.Core.Authorization;
using trembonWoW.Core.Connectors.Auth;
using trembonWoW.Core.Connectors.Characters;
using trembonWoW.Core.Connectors.RemoteAccess;
using trembonWoW.Pages.Files;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddHttpClient("soap", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Soap:Url"]!);

    var authenticationString = $"{builder.Configuration["Soap:Username"]}:{builder.Configuration["Soap:Password"]}";
    var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
    client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
});

builder.Services.AddKeyedMySqlDataSource("auth", builder.Configuration.GetConnectionString("AuthDB")!);
builder.Services.AddKeyedMySqlDataSource("characters", builder.Configuration.GetConnectionString("CharactersDB")!);
builder.Services.AddTransient<IAuthDatabase, AuthDatabase>();
builder.Services.AddTransient<ICharactersDatabase, CharactersDatabase>();
builder.Services.AddTransient<IRemoteAccessSoapAPI, RemoteAccessSoapAPI>();

builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();

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

builder.Services.AddTransient<IUserStore<WoWUser>, WoWUserStore>();
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
app.MapControllers();

app.Run();