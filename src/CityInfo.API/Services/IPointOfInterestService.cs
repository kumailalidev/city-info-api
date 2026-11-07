using CityInfo.API.Models;

namespace CityInfo.API.Services;

/// <summary>
/// Provides operations for managing points of interest for a city.
/// </summary>
public interface IPointOfInterestService
{
    /// <summary>
    /// Creates a new point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterest">The point of interest information to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the create operation.</returns>
    Task<PointOfInterestForCreationResult> CreatePointOfInterestAsync(int cityId,
        PointOfInterestForCreationDto pointOfInterest,
        CancellationToken cancellationToken);
}
