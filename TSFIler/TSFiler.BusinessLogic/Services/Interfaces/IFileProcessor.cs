namespace TSFiler.BusinessLogic.Services.Interfaces;

public interface IFileProcessor
{
    Task<string> ReadFileAsync(Stream fileStream);
    Task WriteFileAsync(Stream outputStream, string content);
}
