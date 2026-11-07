using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.EntityFrameworkCore;

namespace CityInfo.API;

public class CityInfoRepository(CityInfoContext context) : ICityInfoRepository
{
    public async Task<IEnumerable<City>> GetCitiesAsync(CancellationToken cancellationToken)
    {
        return await context.Cities
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<City>> GetCitiesReadOnlyAsync(CancellationToken cancellationToken)
    {
        return await context.Cities.AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

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

    public async Task<bool> CityExistsAsync(int cityId, CancellationToken cancellationToken)
    {
        return await context.Cities.AnyAsync(c => c.Id == cityId, cancellationToken);
    }

    public async Task<City?> GetCityAsync(int cityId,
        bool includePointsOfInterest,
        CancellationToken cancellationToken)
    {
        if (includePointsOfInterest)
        {
            return await context.Cities.Include(c => c.PointsOfInterest) // Eager loading
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await context.Cities.Where(c => c.Id == cityId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddPointOfInterestForCityAsync(int cityId,
        PointOfInterest pointOfInterest,
        CancellationToken cancellationToken)
    {
        var city = await GetCityAsync(cityId, false, cancellationToken);
        city?.PointsOfInterest.Add(pointOfInterest);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        context.PointOfInterests.Remove(pointOfInterest);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) >= 0;
    }

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

    public async Task<int> DeleteAllPointOfInterestForCityAsync(int cityId,
        CancellationToken cancellationToken)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}