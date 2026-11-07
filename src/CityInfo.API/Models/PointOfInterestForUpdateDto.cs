using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

/// <summary>
/// Represents the data required to update a point of interest.
/// </summary>
public class PointOfInterestForUpdateDto
{
    /// <summary>
    /// Gets or sets the name of the point of interest.
    /// </summary>
    [Required(ErrorMessage = "You must provide a name value")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the point of interest.
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }
}