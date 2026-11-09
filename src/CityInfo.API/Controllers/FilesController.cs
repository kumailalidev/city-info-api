using Asp.Versioning;

using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

/// <summary>
/// Provides endpoints for retrieving and uploading files.
/// </summary>
[Route("api/v{version:apiVersion}/files")]
[ApiController]
[ApiVersion(1)] // default
public class FilesController(IFileService fileService) : ControllerBase
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
        var file = await fileService.GetFileAsync(fileId, cancellationToken);
        if (file is null)
        {
            return NotFound();
        }

        return File(file.Content, file.ContentType, file.FileName);
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
        if (!await fileService.SaveFileAsync(file, cancellationToken))
        {
            return BadRequest("No file or an invalid one has been inputted.");
        }

        return Ok("Your file has been uploaded successfully");
    }
}