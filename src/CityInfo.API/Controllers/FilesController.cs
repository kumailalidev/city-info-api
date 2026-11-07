using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[Route("api/files")]
[ApiController]
public class FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider) : ControllerBase
{
    [HttpGet("{fileId}")]
    [ApiVersion(0.1, Deprecated = true)] // Lower number is given because API is deprecated
    public async Task<ActionResult> GetFile(string fileId, CancellationToken cancellationToken = default)
    {
        // look up the actual file, depending on the fileId
        // demo code
        var pathToFile = "SampleTextFile.txt";

        // check whether the file exists
        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }

        if (!fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var bytes = await System.IO.File.ReadAllBytesAsync(pathToFile, cancellationToken);
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }

    [HttpPost]
    public async Task<ActionResult> CreateFile(IFormFile file, CancellationToken cancellationToken = default) // `IFormFile` represents file sent with http req.
    {
        // Validate the input.
        if (file.Length is 0
            || file.Length > 20971520
            || file.ContentType != "text/plain")
        {
            return BadRequest("No file or an invalid one has been inputted.");
        }

        // Create the file path.
        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            $"uploaded_file_{Guid.NewGuid()}.txt");

        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);
        return Ok("Your file has been uploaded successfully");
    }
}