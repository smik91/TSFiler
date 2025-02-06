using Microsoft.Extensions.DependencyInjection;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.DataProcessors;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;
using TSFiler.Common.Exceptions;

namespace TSFiler.BusinessLogic.Factories;

public class DataProcessorFactory : IDataProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DataProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDataProcessor GetDataProcessor(ProcessType processType)
    {
        return processType switch
        {
            ProcessType.Default => _serviceProvider.GetRequiredService<BasicDataProcessor>(),
            ProcessType.Regex => _serviceProvider.GetRequiredService<RegexDataProcessor>(),
            ProcessType.Lib => _serviceProvider.GetRequiredService<LibDataProcessor>(),
            _ => throw new NotSupportedException($"Обработчик данных для {processType} не найден.")
        };
    }
}
