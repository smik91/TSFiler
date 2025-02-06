using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Factories;
using TSFiler.BusinessLogic.Services.DataProcessors;
using TSFiler.BusinessLogic.Services.FileProcessors;
using TSFiler.BusinessLogic.Services;

namespace TSFiler.BusinessLogic;

public static class BusinessLogicServicesExtensions
{
    public static IServiceCollection ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<PlainTextFileProcessor>();
        services.AddTransient<JsonFileProcessor>();
        services.AddTransient<XmlFileProcessor>();
        services.AddTransient<YamlFileProcessor>();

        services.AddTransient<BasicDataProcessor>();
        services.AddTransient<RegexDataProcessor>();
        services.AddTransient<LibDataProcessor>();

        services.AddSingleton<IFileProcessorFactory, FileProcessorFactory>();
        services.AddSingleton<IDataProcessorFactory, DataProcessorFactory>();

        services.AddTransient<FileService>();
        return services;
    }
}
