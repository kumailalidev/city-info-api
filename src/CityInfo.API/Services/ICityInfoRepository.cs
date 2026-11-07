using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Services;

/// <summary>
/// Defines the contract for data access operations related to cities and their points of interest.
/// </summary>
public interface ICityInfoRepository
{
    /// <summary>
    /// Gets all cities asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of cities.</returns>
    Task<IEnumerable<City>> GetCitiesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets all cities in a read-only form asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A read-only collection of cities.</returns>
    Task<IEnumerable<City>> GetCitiesReadOnlyAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paginated, filtered set of cities asynchronously.
    /// </summary>
    /// <param name="name">Optional city name filter.</param>
    /// <param name="searchQuery">Optional search query used to match city data.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the matching cities and pagination metadata.</returns>
    Task<(IEnumerable<City>, PaginationMetadata?)> GetCitiesReadOnlyAsync(
        string? name,
        string? searchQuery,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether a city exists asynchronously.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the city exists; otherwise, <c>false</c>.</returns>
    Task<bool> CityExistsAsync(int cityId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a city by identifier, optionally including its points of interest.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="includePointsOfInterest">A flag indicating whether to include points of interest.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The city if found; otherwise, <c>null</c>.</returns>
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all points of interest for a city asynchronously.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of points of interest for the city.</returns>
    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a specific point of interest for a city asynchronously.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point of interest identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The point of interest if found; otherwise, <c>null</c>.</returns>
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Adds a point of interest to a city asynchronously.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterest">The point of interest to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddPointOfInterestForCityAsync(int cityId,
        PointOfInterest pointOfInterest,
        CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a point of interest from the in-memory collection.
    /// </summary>
    /// <param name="pointOfInterest">The point of interest to delete.</param>
    void DeletePointOfInterest(PointOfInterest pointOfInterest);

    /// <summary>
    /// Updates the description of all points of interest for a city asynchronously.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="newDescription">The new description to assign.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of points of interest updated.</returns>
    Task<int> UpdatePointsPointOfInterestDescriptionForCityAsync(int cityId,
        string newDescription,
        CancellationToken cancellationToken);

    /// <summary>
    /// Deletes all points of interest for a city asynchronously.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of points of interest deleted.</returns>
    Task<int> DeleteAllPointOfInterestForCityAsync(int cityId, CancellationToken cancellationToken);

    /// <summary>
    /// Saves all pending repository changes asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if changes were successfully saved; otherwise, <c>false</c>.</returns>
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}