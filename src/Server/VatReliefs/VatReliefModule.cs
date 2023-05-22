using Carter;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BirToolsApp.Server.VatReliefs;

public class VatReliefModule : CarterModule
{
    public VatReliefModule() : base("api/vat-relief")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async Task<Results<BadRequest<string>, FileContentHttpResult>> (IFormFile? file, VatReliefService service) =>
        {
            if (file == null) return TypedResults.BadRequest("Invalid excel file");

            var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            var output = await service.Process(ms);
            if (output.Success)
                return TypedResults.File(output.File, "application/zip", output.FileName);

            return TypedResults.BadRequest(output.ErrorMessage);
        });
    }
}