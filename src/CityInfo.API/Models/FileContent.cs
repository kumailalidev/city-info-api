namespace CityInfo.API.Models;

/// <summary>
/// Contains the data needed to return a file to a client.
/// </summary>
/// <param name="Content">The file bytes.</param>
/// <param name="ContentType">The MIME type of the file.</param>
/// <param name="FileName">The name of the file.</param>
public sealed record FileContent(byte[] Content, string ContentType, string FileName);
