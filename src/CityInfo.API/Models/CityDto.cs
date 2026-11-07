namespace CityInfo.API.Models;

/// <summary>
/// Represents a city returned by the API.
/// </summary>
public class CityDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the city.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a description of the city.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets the total number of points of interest in the city.
    /// </summary>
    public int NumberOfPointsOfInterest => PointsOfInterest.Count;

    /// <summary>
    /// Gets or sets the collection of points of interest for the city.
    /// </summary>
    public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = [];
}