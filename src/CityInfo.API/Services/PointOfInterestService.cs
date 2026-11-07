using AutoMapper;

using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Services;

/// <summary>
/// Provides operations for creating and managing city points of interest.
/// </summary>
public class PointOfInterestService(ICityInfoRepository repository,
    IMapper mapper,
    IMailService mailService) : IPointOfInterestService
{
    /// <summary>
    /// Creates a new point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The identifier of the city.</param>
    /// <param name="pointOfInterest">The point of interest details to create.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>The result of the create operation.</returns>
    public async Task<PointOfInterestForCreationResult> CreatePointOfInterestAsync(int cityId,
        PointOfInterestForCreationDto pointOfInterest,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return PointOfInterestForCreationResult.Failed("City not found");
        }

        // POIs count validation
        // TODO: improve memory and performance by not returning the POIs
        var existingPOIs = await repository.GetPointsOfInterestForCityAsync(cityId,
            cancellationToken);
        if (existingPOIs.Count() >= 10)
        {
            return PointOfInterestForCreationResult.Failed(
                "City has reached maximum capacity of 10 points of interest");
        }

        var pointOfInterestEntity = mapper.Map<PointOfInterest>(pointOfInterest);
        await repository.AddPointOfInterestForCityAsync(cityId, pointOfInterestEntity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await SendCreationNotificationAsync(cityId, pointOfInterestEntity);

        var resultDto = mapper.Map<PointOfInterestDto>(pointOfInterestEntity);
        return PointOfInterestForCreationResult.Successful(resultDto);
    }

    // Helper for sending notifications
    private async Task SendCreationNotificationAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var subject = "New Point of Interest Added";
        var message = $"A new point of interest '{pointOfInterest.Name} has been added to city {cityId}'";
        mailService.Send(subject, message);
    }
}