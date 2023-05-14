using System.Text.Json;
using BirToolsApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BirToolsApp.Server.Controllers;

[ApiController]
[Route("withholding-agents")]
public class WithholdingAgentController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WithholdingAgentController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<WithholdingAgent>> Get()
    {
        var data = await GetDataAsync();
        return data.OrderBy(x => x.DatePublished).ThenBy(x => x.Name);
    }

    private async Task<IEnumerable<WithholdingAgent>> GetDataAsync()
    {
        try
        {
            var json = await System.IO.File.ReadAllTextAsync("Data/top-agents.json");

            var data = JsonSerializer.Deserialize<IEnumerable<WithholdingAgent>>(json);
            return data ?? Array.Empty<WithholdingAgent>();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "{}", e.Message);
            return Array.Empty<WithholdingAgent>();
        }
    }

    [HttpGet("search")]
    public async Task<IEnumerable<WithholdingAgent>> Search(string term)
    {
        var data = await GetDataAsync();

        return data.Where(x =>
                x.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Rdo.Contains(term, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.DatePublished)
            .ThenBy(x => x.Name);
    }
}