using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

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
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = name;
    [MaxLength(200)]
    public string? Description { get; set; }

    [ForeignKey("CityId")]
    public City? City { get; set; }
    public int CityId { get; set; }
}