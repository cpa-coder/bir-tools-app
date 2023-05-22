using Carter;

namespace BirToolsApp.Server.WithholdingAgents;

public class WithholdingAgentModule : CarterModule
{
    public WithholdingAgentModule() : base("api/withholding-agents")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (WithholdingAgentService service) => await service.GetAllAsync());
        app.MapGet("/{term}", async (WithholdingAgentService service, string term) => await service.SearchAsync(term));
    }
}