namespace CityInfo.API.Models;

/// <summary>
/// Represents the data required to update one or more points of interest in bulk.
/// </summary>
public class PointsOfInterestBulkUpdateDto
{
    /// <summary>
    /// Gets or sets the new description to apply to the points of interest.
    /// </summary>
    public string NewDescription { get; set; } = string.Empty;
}