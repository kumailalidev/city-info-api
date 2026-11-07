using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

/// <summary>
/// Represents a point of interest within a city.
/// </summary>
/// <param name="name">The name of the point of interest.</param>
public class PointOfInterest(string name)
{
    /**
     *  NOTES:
     *      - Id property is auto. regarded as primary key
     *      - [Key] attribute is not required, because Id is auto. regarded as primary key
     *      - [DatabaseGenerated(DatabaseGeneratedOption.Identity)] attribute not required, because Id is auto. generated as per convention
     *      - City property helps in navigating object graph i.e parent `City` object; targets City.Id by default fot navigation
     *      - CityId is not required, foreign key is auto. added as CityId as per convention
     *      - [ForeignKey("CityId")] attribute id not required; foreign key is auto. added as CityId as per convention
    */

    /// <summary>
    /// Gets or sets the unique identifier for the point of interest.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the point of interest.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets a description of the point of interest.
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the city this point of interest belongs to.
    /// </summary>
    [ForeignKey("CityId")]
    public City? City { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the city this point of interest belongs to.
    /// </summary>
    public int CityId { get; set; }
}