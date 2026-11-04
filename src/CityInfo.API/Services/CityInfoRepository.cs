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

    public async Task<IEnumerable<City>> GetCitiesReadOnlyAsync()
    {
        return await context.Cities.AsNoTracking()
            .OrderBy(c => c.Name).ToListAsync();
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
}
