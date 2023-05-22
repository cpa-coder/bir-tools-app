using System.Text.Json;
using BirToolsApp.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BirToolsApp.Server.WithholdingAgents;

public class WithholdingAgentService
{
    private readonly ILogger<WithholdingAgentService> _logger;

    public WithholdingAgentService(ILogger<WithholdingAgentService> logger)
    {
        _logger = logger;
    }

    public async Task<Results<Ok<IEnumerable<WithholdingAgent>>, NotFound<string>>> GetAllAsync()
    {
        var data = await GetDataAsync();
        if (data == null)
            return TypedResults.NotFound("No available data");

        return TypedResults.Ok(data.OrderBy(x => x.DatePublished).ThenBy(x => x.Name).AsEnumerable());
    }

    private async Task<IEnumerable<WithholdingAgent>?> GetDataAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync("Data/top-agents.json");
            var data = JsonSerializer.Deserialize<IEnumerable<WithholdingAgent>>(json);
            return data;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "{}", e.Message);
            return null;
        }
    }

    public async Task<Results<Ok<IEnumerable<WithholdingAgent>>, NotFound<string>>> SearchAsync(string term)
    {
        var data = await GetDataAsync();
        if (data == null)
            return TypedResults.NotFound("No available data");

        return TypedResults.Ok(data.Where(x =>
                x.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Rdo.Contains(term, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.DatePublished)
            .ThenBy(x => x.Name).AsEnumerable());
    }
}