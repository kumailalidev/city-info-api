using AutoMapper;

using CityInfo.API;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;

public class PointOfInterestService(ICityInfoRepository repository,
    IMapper mapper,
    IMailService mailService) : IPointOfInterestService
{
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