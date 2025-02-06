using Microsoft.Extensions.DependencyInjection;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.FileProcessors;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Factories;

public class FileProcessorFactory : IFileProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FileProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFileProcessor GetFileProcessor(FileType fileType)
    {
        return fileType switch
        {
            FileType.Txt => _serviceProvider.GetRequiredService<PlainTextFileProcessor>(),
            FileType.Json => _serviceProvider.GetRequiredService<JsonFileProcessor>(),
            FileType.Xml => _serviceProvider.GetRequiredService<XmlFileProcessor>(),
            FileType.Yaml => _serviceProvider.GetRequiredService<YamlFileProcessor>(),
            _ => throw new NotSupportedException($"Обработчик файлов для \"{fileType}\" не найден.")
        };
    }
}
