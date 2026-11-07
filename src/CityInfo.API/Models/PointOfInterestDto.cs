namespace CityInfo.API.Models;

/// <summary>
/// Represents a point of interest in a city.
/// </summary>
public class PointOfInterestDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the point of interest.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the point of interest.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the point of interest.
    /// </summary>
    public string? Description { get; set; }
}