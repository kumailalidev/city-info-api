using CityInfo.API.Models;

using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Services;

/// <summary>
/// Handles file retrieval and storage.
/// </summary>
public class FileService(FileExtensionContentTypeProvider fileExtensionContentTypeProvider) : IFileService
{
    /// <inheritdoc />
    public async Task<FileContent?> GetFileAsync(string fileId, CancellationToken cancellationToken)
    {
        var pathToFile = "SampleTextFile.txt";
        if (!System.IO.File.Exists(pathToFile))
        {
            return null;
        }

        if (!fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var bytes = await System.IO.File.ReadAllBytesAsync(pathToFile, cancellationToken);
        return new FileContent(bytes, contentType, Path.GetFileName(pathToFile));
    }

    /// <inheritdoc />
    public async Task<bool> SaveFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length is 0
            || file.Length > 20971520
            || file.ContentType != "text/plain")
        {
            return false;
        }

        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            $"uploaded_file_{Guid.NewGuid()}.txt");

        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);
        return true;
    }
}
