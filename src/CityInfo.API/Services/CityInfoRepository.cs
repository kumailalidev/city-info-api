using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Services;

using Microsoft.EntityFrameworkCore;

namespace CityInfo.API;

public class CityInfoRepository(CityInfoContext context) : ICityInfoRepository
{
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await context.Cities.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<IEnumerable<City>> GetCitiesReadOnlyAsync(CancellationToken cancellationToken)
    {
        return await context.Cities.AsNoTracking()
            .OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<City>> GetCitiesReadOnlyAsync(string? name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await GetCitiesReadOnlyAsync(cancellationToken);
        }

        name = name.Trim();

        return await context.Cities
            .AsNoTracking()
            .Where(c => c.Name == name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
    {
        if (includePointsOfInterest)
        {
            return await context.Cities.Include(c => c.PointsOfInterest) // Eager loading
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync();
        }

        return await context.Cities.Where(c => c.Id == cityId)
            .FirstOrDefaultAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ToListAsync();
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        city?.PointsOfInterest.Add(pointOfInterest);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        context.PointOfInterests.Remove(pointOfInterest);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }

    public async Task<int> UpdatePointsPointOfInterestDescriptionForCityAsync(int cityId, string newDescription)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Description, newDescription)); // can be chained to multiple properties
    }

    public async Task<int> DeleteAllPointOfInterestForCityAsync(int cityId)
    {
        return await context.PointOfInterests
            .Where(p => p.CityId == cityId)
            .ExecuteDeleteAsync();
    }
}