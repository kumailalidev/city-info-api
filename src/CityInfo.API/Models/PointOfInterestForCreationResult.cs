namespace CityInfo.API.Models;

public class PointOfInterestForCreationResult
{
    public static PointOfInterestForCreationResult Successful(PointOfInterestDto? pointOfInterest) =>
        new()
        {
            Success = true,
            PointOfInterest = pointOfInterest
        };

    public static PointOfInterestForCreationResult Failed(string error) =>
        new()
        {
            Success = false,
            ErrorMessage = error
        };

    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public PointOfInterestDto? PointOfInterest { get; set; }
}