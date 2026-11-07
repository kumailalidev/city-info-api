namespace CityInfo.API.Models;

/// <summary>
/// Represents pagination metadata for a paged result.
/// </summary>
public class PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
{
    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int TotalItemCount { get; set; } = totalItemCount;

    /// <summary>
    /// Gets the total number of pages based on the current page size.
    /// </summary>
    public int TotalPageCount { get; set; } = (int)Math.Ceiling(totalItemCount / (double)pageSize);

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int pageSize { get; set; } = pageSize;

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int currentPage { get; set; } = currentPage;
}