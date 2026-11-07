using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

/// <summary>
/// Represents a city with a name, optional description, and related points of interest.
/// </summary>
/// <param name="name">The name of the city.</param>
public class City(string name)
{
    /**
     *  NOTES:
     *      - Id property is auto. regarded as primary key
     *      - [Key] attribute is not required, because Id is auto. regarded as primary key
     *      - [DatabaseGenerated(DatabaseGeneratedOption.Identity)] attribute not required, because Id is auto. generated as per convention
    */

    /// <summary>
    /// Gets or sets the unique identifier for the city.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets the optional description of the city.
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the collection of points of interest associated with the city.
    /// </summary>
    public ICollection<PointOfInterest> PointsOfInterest { get; set; } = [];
}