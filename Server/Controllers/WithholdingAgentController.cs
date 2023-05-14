using System.Text.Json;
using BirToolsApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BirToolsApp.Server.Controllers;

[ApiController]
[Route("withholding-agents")]
public class WithholdingAgentController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WithholdingAgent>> Get()
    {
        var json = await System.IO.File.ReadAllTextAsync("data/top-agents.json");

        var data = JsonSerializer.Deserialize<IEnumerable<WithholdingAgent>>(json);
        if (data == null) return Array.Empty<WithholdingAgent>();

        return data.OrderBy(x => x.DatePublished).ThenBy(x => x.Name);
    }

    [HttpGet("search")]
    public async Task<IEnumerable<WithholdingAgent>> Search(string term)
    {
        var json = await System.IO.File.ReadAllTextAsync("data/top-agents.json");

        var data = JsonSerializer.Deserialize<IEnumerable<WithholdingAgent>>(json);
        if (data == null) return Array.Empty<WithholdingAgent>();

        return data.Where(x =>
                x.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Rdo.Contains(term, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.DatePublished)
            .ThenBy(x => x.Name);
    }
}