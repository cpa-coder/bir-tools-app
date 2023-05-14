using System.Reflection;
using BirToolsApp.Server;
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

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// ClosedXml font fallback when generating excel files
await using (var fallbackFontStream =
             Assembly.GetExecutingAssembly().GetManifestResourceStream("BirToolsApp.Server.Data.Carlito-Regular.ttf"))
{
    LoadOptions.DefaultGraphicEngine = DefaultGraphicEngine.CreateOnlyWithFonts(fallbackFontStream);
}

app.Run();