using CompanyResources.UI;
using CompanyResources.UI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Tutaj wpisz swój port API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7153") });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
