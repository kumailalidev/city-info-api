namespace CityInfo.API.Models;

/// <summary>
/// Represents the result of creating a point of interest.
/// </summary>
public class PointOfInterestForCreationResult
{
    /// <summary>
    /// Creates a successful result for a point of interest creation request.
    /// </summary>
    /// <param name="pointOfInterest">The created point of interest, if any.</param>
    /// <returns>A successful creation result.</returns>
    public static PointOfInterestForCreationResult Successful(PointOfInterestDto? pointOfInterest) =>
        new()
        {
            Success = true,
            PointOfInterest = pointOfInterest
        };

    /// <summary>
    /// Creates a failed result for a point of interest creation request.
    /// </summary>
    /// <param name="error">The error message describing the failure.</param>
    /// <returns>A failed creation result.</returns>
    public static PointOfInterestForCreationResult Failed(string error) =>
        new()
        {
            Success = false,
            ErrorMessage = error
        };

    /// <summary>
    /// Gets or sets a value indicating whether the creation operation succeeded.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the error message for a failed creation operation.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the created point of interest.
    /// </summary>
    public PointOfInterestDto? PointOfInterest { get; set; }
}