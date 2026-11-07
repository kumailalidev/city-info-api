using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Models;

using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

/// <summary>
/// Provides data access methods for cities and points of interest.
/// </summary>
public class CityInfoRepository(CityInfoContext context) : ICityInfoRepository
{
    /// <summary>
    /// Gets all cities ordered by name.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A collection of cities.</returns>
    public async Task<IEnumerable<City>> GetCitiesAsync(CancellationToken cancellationToken)
    {
        return await context.Cities
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets all cities without change tracking.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A collection of cities.</returns>
    public async Task<IEnumerable<City>> GetCitiesReadOnlyAsync(CancellationToken cancellationToken)
    {
        return await context.Cities.AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a filtered, paginated set of cities without change tracking.
    /// </summary>
    /// <param name="name">The city name to match.</param>
    /// <param name="searchQuery">A text fragment used to search city names and descriptions.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of records on a page.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The cities for the current page and pagination metadata.</returns>
    public async Task<(IEnumerable<City>, PaginationMetadata?)> GetCitiesReadOnlyAsync(
        string? name,
        string? searchQuery,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        // Casting IQueryable to City DbSet (for deferred execution; execution is
        // deferred until the query is iterated over via ToList(), ToArray(), etc)
        var collection = context.Cities as IQueryable<City>;

        if (!string.IsNullOrEmpty(name))
        {
            name = name.Trim();
            collection = collection.Where(c => c.Name == name);
        }

        // Search across name and description of city
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection
                .Where(c => c.Name.Contains(searchQuery)
                    || (c.Description != null && c.Description.Contains(searchQuery)));
        }

        var totalItemCount = await collection.CountAsync(cancellationToken);
        var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

        var collectionToReturn = await collection
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (collectionToReturn, paginationMetadata);
    }

    /// <summary>
    /// Determines whether a city exists.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns><c>true</c> if the city exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> CityExistsAsync(int cityId, CancellationToken cancellationToken)
    {
        return await context.Cities.AnyAsync(c => c.Id == cityId, cancellationToken);
    }

    /// <summary>
    /// Gets a city by identifier, optionally including its points of interest.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="includePointsOfInterest">A value indicating whether to include the city points of interest.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The city, if found; otherwise, <c>null</c>.</returns>
    public async Task<City?> GetCityAsync(int cityId,
        bool includePointsOfInterest,
        CancellationToken cancellationToken)
    {
        var query = context.Cities.AsQueryable();

        if (includePointsOfInterest)
        {
            query = query.Include(c => c.PointsOfInterest); // Eager loading
        }

        return await query
            .Where(c => c.Id == cityId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a point of interest for a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point of interest identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The point of interest, if found; otherwise, <c>null</c>.</returns>
    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Gets all points of interest for a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A collection of points of interest.</returns>
    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new point of interest to a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterest">The point of interest to add.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public async Task AddPointOfInterestForCityAsync(int cityId,
        PointOfInterest pointOfInterest,
        CancellationToken cancellationToken)
    {
        var city = await GetCityAsync(cityId, false, cancellationToken);
        city?.PointsOfInterest.Add(pointOfInterest);
    }

    /// <summary>
    /// Removes a point of interest from the context.
    /// </summary>
    /// <param name="pointOfInterest">The point of interest to delete.</param>
    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        context.PointOfInterests.Remove(pointOfInterest);
    }

    /// <summary>
    /// Saves all changes made in the repository context.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns><c>true</c> if the save succeeded; otherwise, <c>false</c>.</returns>
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) >= 0;
    }

    /// <summary>
    /// Updates the description of all points of interest for a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="newDescription">The new description value.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The number of records updated.</returns>
    public async Task<int> UpdatePointsPointOfInterestDescriptionForCityAsync(int cityId,
        string newDescription,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Description, newDescription),
                cancellationToken); // can be chained to multiple properties
    }

    /// <summary>
    /// Deletes all points of interest for a city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The number of records deleted.</returns>
    public async Task<int> DeleteAllPointOfInterestForCityAsync(int cityId,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}