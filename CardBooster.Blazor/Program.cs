using CardBooster.Blazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using CardBooster.Blazor.Services;
using CardBooster.Blazor.Services.Interface;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("CardBoosterAPI",
    client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]))
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CardBoosterAPI"));

await builder.Build().RunAsync();

