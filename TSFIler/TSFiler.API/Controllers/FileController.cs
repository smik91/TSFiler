using Microsoft.AspNetCore.Mvc;
using TSFiler.BusinessLogic.Services;
using TSFiler.Common.Enums;

namespace TSFiler.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessFile([FromQuery] string outputFileName, [FromQuery] ProcessType processType, IFormFile file)
    {
        var result = await _fileService.ProcessFileAsync(outputFileName, processType, file);
        return File(result.FileContents, result.MimeType, result.FileName);
    }
}
