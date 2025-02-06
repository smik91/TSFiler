namespace TSFiler.BusinessLogic.Models;

public class ProcessedFileResult
{
    public byte[] FileContents { get; set; }
    public string MimeType { get; set; }
    public string FileName { get; set; }
}
