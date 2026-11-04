using CityInfo.API.Models;

namespace CityInfo.API.Services;

public interface IPointOfInterestService
{
    Task<PointOfInterestForCreationResult> CreatePointOfInterestAsync(int cityId,
        PointOfInterestForCreationDto pointOfInterest,
        CancellationToken cancellationToken);
}
