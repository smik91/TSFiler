using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Models;
using TSFiler.Common.Enums;
using TSFiler.Common.Exceptions;
using TSFiler.Common.Helpers;

namespace TSFiler.BusinessLogic.Services;

public class FileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IFileProcessorFactory _fileProcessorFactory;
    private readonly IDataProcessorFactory _dataProcessorFactory;

    public FileService(ILogger<FileService> logger, IFileProcessorFactory fileProcessorFactory, IDataProcessorFactory dataProcessorFactory)
    {
        _logger = logger;
        _fileProcessorFactory = fileProcessorFactory;
        _dataProcessorFactory = dataProcessorFactory;
    }

    public async Task<ProcessedFileResult> ProcessFileAsync(string outputFileName, ProcessType processType, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new IncorrectDataException("Файл не загружен либо пуст.");
        }

        _logger.LogInformation("Получен файл: {FileName}, Имя выходного файла: {OutputFileName}, Тип обработки: {ProcessType}",
            file.FileName, outputFileName, processType);

        using var inputStream = file.OpenReadStream();
        using var outputStream = new MemoryStream();

        var fileType = FileTypeDeterminator.GetFileType(file.FileName);
        var fileProcessor = _fileProcessorFactory.GetFileProcessor(fileType);
        var dataProcessor = _dataProcessorFactory.GetDataProcessor(processType);

        string data = await fileProcessor.ReadFileAsync(inputStream);
        string processedData = dataProcessor.ProcessData(data);
        await fileProcessor.WriteFileAsync(outputStream, processedData);

        var mimeType = fileType switch
        {
            FileType.Txt => "text/plain",
            FileType.Json => "application/json",
            FileType.Xml => "application/xml",
            FileType.Yaml => "application/x-yaml",
            _ => "text/plain"
        };

        var processedFileResult = new ProcessedFileResult
        {
            FileContents = outputStream.ToArray(),
            MimeType = mimeType,
            FileName = $"{outputFileName}.{fileType.ToString().ToLower()}"
        };

        return processedFileResult;
    }
}
