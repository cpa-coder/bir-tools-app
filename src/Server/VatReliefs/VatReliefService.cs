using System.IO.Compression;
using BirToolsApp.Server.VatReliefs.Utils;

namespace BirToolsApp.Server.VatReliefs;

public class VatReliefService
{
    public async Task<ReliefOutput> Process(MemoryStream excelStream)
    {
        var reader = new VatTemplateReader();
        var result = await reader.ReadAsync(excelStream);

        if (result.IsFaulted) return new ReliefOutput { ErrorMessage = result.ToString() };

        var baseFolder = Path.Combine(Path.GetTempPath(), "vat-relief");
        var workingFolder = Guid.NewGuid().ToString().Replace("-", "");
        var tempFolder = Path.Combine(baseFolder, workingFolder);
        Directory.CreateDirectory(tempFolder);

        var generator = new DatFileGenerator();
        var generateResult = await generator.GenerateAsync(result.Value, tempFolder);
        if (generateResult.IsFaulted) return new ReliefOutput { ErrorMessage = generateResult.ToString() };

        var reconWriter = new ExcelReconWriter();
        var writeResult = await reconWriter.WriteReconciliationReportAsync(result.Value, tempFolder);

        if (writeResult.IsFaulted) return new ReliefOutput { ErrorMessage = writeResult.ToString() };

        var fileName = $"{workingFolder}.zip";
        var fileUrl = Path.Combine(baseFolder, fileName);

        ZipFile.CreateFromDirectory(tempFolder, fileUrl);

        var fileBytes = await File.ReadAllBytesAsync(fileUrl);

        if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
        if (File.Exists(fileUrl)) File.Delete(fileUrl);

        return new ReliefOutput { File = fileBytes, FileName = fileName, Success = true };
    }
}

public record ReliefOutput
{
    public byte[] File { get; init; } = Array.Empty<byte>();
    public string FileName { get; init; } = string.Empty;
    public bool Success { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
}