using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Factories.Interfaces;

public interface IDataProcessorFactory
{
    IDataProcessor GetDataProcessor(ProcessType processType);
}
