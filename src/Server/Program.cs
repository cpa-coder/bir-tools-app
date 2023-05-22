using System.Reflection;
using System.Runtime.InteropServices;
using BirToolsApp.Server.Utilities;
using BirToolsApp.Server.VatReliefs;
using BirToolsApp.Server.WithholdingAgents;
using Carter;
using ClosedXML.Excel;
using ClosedXML.Graphics;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
    builder.WebHost.ConfigureKestrel(options =>
    {
        var ip = IpAddressHelper.GetIpAddress();

        options.Listen(System.Net.IPAddress.Parse(ip), 5001);
        options.Listen(System.Net.IPAddress.Parse("127.0.0.1"), 5000);
    });

var windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
if (windows) builder.Host.UseWindowsService();

//StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddCarter();

builder.Services.AddSingleton<VatReliefService>();
builder.Services.AddSingleton<WithholdingAgentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapCarter();
app.MapRazorPages();
app.MapFallbackToFile("index.html");

// ClosedXml font fallback when generating excel files
const string fontName = "BirToolsApp.Server.Data.Carlito-Regular.ttf";
await using (var fallbackFontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontName))
    LoadOptions.DefaultGraphicEngine = DefaultGraphicEngine.CreateOnlyWithFonts(fallbackFontStream);

app.Run();