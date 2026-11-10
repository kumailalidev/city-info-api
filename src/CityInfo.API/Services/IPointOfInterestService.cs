using CityInfo.API.Models;

namespace CityInfo.API.Services;

/// <summary>
/// Provides operations for managing points of interest for a city.
/// </summary>
public interface IPointOfInterestService
{
    /// <summary>
    /// Gets all points of interest for a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The points of interest, or <c>null</c> if the city does not exist.</returns>
    Task<IEnumerable<PointOfInterestDto>?> GetPointsOfInterestForCityAsync(
        int cityId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a point of interest for a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point-of-interest identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The point of interest, or <c>null</c> if it does not exist.</returns>
    Task<PointOfInterestDto?> GetPointOfInterestForCityAsync(
        int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a point of interest in the format used for updates.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point-of-interest identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The update DTO, or <c>null</c> if the point of interest does not exist.</returns>
    Task<PointOfInterestForUpdateDto?> GetPointOfInterestForUpdateAsync(
        int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken);

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

    /// <summary>
    /// Deletes a point of interest from a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point-of-interest identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the point of interest was deleted; otherwise, <c>false</c>.</returns>
    Task<bool> DeletePointOfInterestAsync(
        int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates a point of interest.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point-of-interest identifier.</param>
    /// <param name="pointOfInterest">The updated point-of-interest data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the point of interest was updated; otherwise, <c>false</c>.</returns>
    Task<bool> UpdatePointOfInterestAsync(
        int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates the description of all points of interest in a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="newDescription">The replacement description.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of affected rows, or <c>null</c> if the city does not exist.</returns>
    Task<int?> BulkUpdatePointsOfInterestDescriptionAsync(
        int cityId,
        string newDescription,
        CancellationToken cancellationToken);
}
