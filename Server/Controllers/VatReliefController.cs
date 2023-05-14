using System.IO.Compression;
using BirToolsApp.Server.VatReliefServices.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BirToolsApp.Server.Controllers;

[ApiController]
[Route("vat-relief")]
public class VatReliefController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ProcessRelief(IFormFile? excelFile)
    {
        if (excelFile == null) return BadRequest("Invalid excel file");

        var ms = new MemoryStream();
        await excelFile.OpenReadStream().CopyToAsync(ms);
        var reader = new VatTemplateReader();
        var result = await reader.ReadAsync(ms);

        if (result.IsFaulted)
        {
            return BadRequest(result.ToString());
        }

        var baseFolder = Path.Combine(Path.GetTempPath(), "vat-relief");
        var workingFolder = Guid.NewGuid().ToString().Replace("-", "");
        var tempFolder = Path.Combine(baseFolder, workingFolder);
        Directory.CreateDirectory(tempFolder);

        var generator = new DatFileGenerator();
        var generateResult = await generator.GenerateAsync(result.Value, tempFolder);
        if (generateResult.IsFaulted)
        {
            return BadRequest(generateResult.ToString());
        }

        var reconWriter = new ExcelReconWriter();
        var writeResult = await reconWriter.WriteReconciliationReportAsync(result.Value, tempFolder);

        if (writeResult.IsFaulted)
        {
            return BadRequest(writeResult.ToString());
        }

        var fileName = $"{workingFolder}.zip";
        var fileUrl = Path.Combine(baseFolder, fileName);

        ZipFile.CreateFromDirectory(tempFolder, fileUrl);

        var fileBytes = await System.IO.File.ReadAllBytesAsync(fileUrl);

        if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
        if (System.IO.File.Exists(fileUrl)) System.IO.File.Delete(fileUrl);

        return File(fileBytes, "application/zip", Path.GetFileName(fileUrl));
    }
}