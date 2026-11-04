using CityInfo.API.Entities;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
    // IQueryable<City> GetCities();
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<IEnumerable<City>> GetCitiesReadOnlyAsync();
    Task<bool> CityExistsAsync(int cityId);
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
        int pointOfInterestId);
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
    void DeletePointOfInterest(PointOfInterest pointOfInterest); // Not async: In-Memory OP,NOT I/O method
    Task<bool> SaveChangesAsync();
}
