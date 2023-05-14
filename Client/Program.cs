using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BirToolsApp.Client;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

var app = builder.Build();

var snackbar = app.Services.GetRequiredService<ISnackbar>();
snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
snackbar.Configuration.ShowTransitionDuration = 300;
snackbar.Configuration.HideTransitionDuration = 300;

await app.RunAsync();