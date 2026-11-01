using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

public class City(string name)
{
    /**
     *  NOTES:
     *      - Id property is auto. regarded as primary key
     *      - [Key] attribute is not required, because Id is auto. regarded as primary key
     *      - [DatabaseGenerated(DatabaseGeneratedOption.Identity)] attribute not required, because Id is auto. generated as per convention
    */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = name;
    [MaxLength(200)]
    public string? Description { get; set; }
    public ICollection<PointOfInterest> PointsOfInterest { get; set; } = [];
}
