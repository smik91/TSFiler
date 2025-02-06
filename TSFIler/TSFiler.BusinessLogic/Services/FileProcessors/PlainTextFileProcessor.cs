using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class PlainTextFileProcessor : IFileProcessor
{
    public async Task<string> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var fileData = await reader.ReadToEndAsync();
        return fileData;
    }

    public async Task WriteFileAsync(Stream outputStream, string content)
    {
        using var writer = new StreamWriter(outputStream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
    }
}
