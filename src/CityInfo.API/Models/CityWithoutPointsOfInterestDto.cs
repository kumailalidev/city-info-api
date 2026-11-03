namespace CityInfo.API.Models;

public class CityWithoutPointsOfInterestDto // DTO shape returned by API
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}