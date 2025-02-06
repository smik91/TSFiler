using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Factories.Interfaces;

public interface IFileProcessorFactory
{
    IFileProcessor GetFileProcessor(FileType fileType);
}
