using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

/// <summary>
/// Provides endpoints for retrieving and uploading files.
/// </summary>
[Route("api/v{version:apiVersion}/files")]
[ApiController]
[ApiVersion(1)] // default
public class FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider) : ControllerBase
{
    /// <summary>
    /// Retrieves a file by its identifier.
    /// </summary>
    /// <param name="fileId">The unique identifier of the file to retrieve.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested file content.</returns>
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

    /// <summary>
    /// Uploads a text file to the server.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A success or validation result.</returns>
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