using CityInfo.API.Models;

namespace CityInfo.API.Services;

/// <summary>
/// Defines file retrieval and storage operations.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Gets the demo file. The file identifier is currently ignored.
    /// </summary>
    /// <param name="fileId">The file identifier.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The file content, or <c>null</c> if the file does not exist.</returns>
    Task<FileContent?> GetFileAsync(string fileId, CancellationToken cancellationToken);

    /// <summary>
    /// Validates and stores an uploaded file.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns><c>true</c> if the file was stored; otherwise, <c>false</c>.</returns>
    Task<bool> SaveFileAsync(IFormFile file, CancellationToken cancellationToken);
}
