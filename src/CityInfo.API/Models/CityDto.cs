namespace CityInfo.API.Models;

public class CityDto // DTO shape returned by API
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int NumberOfPointsOfInterest => PointsOfInterest.Count;
    public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = [];
}