using System.Xml.Linq;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class XmlFileProcessor : IFileProcessor
{
    public async Task<string> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var fileData = await reader.ReadToEndAsync();
        return fileData;
    }

    public async Task WriteFileAsync(Stream outputStream, string content)
    {
        var xmlDocument = XDocument.Parse(content);
        await Task.Run(() => xmlDocument.Save(outputStream));
    }
}
