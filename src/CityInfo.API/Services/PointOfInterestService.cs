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
    /// <inheritdoc />
    public async Task<IEnumerable<PointOfInterestDto>?> GetPointsOfInterestForCityAsync(
        int cityId,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return null;
        }

        var pointsOfInterest = await repository.GetPointsOfInterestForCityAsync(cityId, cancellationToken);
        return mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest);
    }

    /// <inheritdoc />
    public async Task<PointOfInterestDto?> GetPointOfInterestForCityAsync(
        int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return null;
        }

        var pointOfInterest = await repository.GetPointOfInterestForCityAsync(
            cityId, pointOfInterestId, cancellationToken);
        return pointOfInterest is null ? null : mapper.Map<PointOfInterestDto>(pointOfInterest);
    }

    /// <inheritdoc />
    public async Task<PointOfInterestForUpdateDto?> GetPointOfInterestForUpdateAsync(
        int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return null;
        }

        var pointOfInterest = await repository.GetPointOfInterestForCityAsync(
            cityId, pointOfInterestId, cancellationToken);
        return pointOfInterest is null
            ? null
            : mapper.Map<PointOfInterestForUpdateDto>(pointOfInterest);
    }

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

        SendCreationNotification(cityId, pointOfInterestEntity);

        var resultDto = mapper.Map<PointOfInterestDto>(pointOfInterestEntity);
        return PointOfInterestForCreationResult.Successful(resultDto);
    }

    /// <inheritdoc />
    public async Task<bool> DeletePointOfInterestAsync(
        int cityId,
        int pointOfInterestId,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return false;
        }

        var pointOfInterest = await repository.GetPointOfInterestForCityAsync(
            cityId, pointOfInterestId, cancellationToken);
        if (pointOfInterest is null)
        {
            return false;
        }

        repository.DeletePointOfInterest(pointOfInterest);
        await repository.SaveChangesAsync(cancellationToken);

        mailService.Send("Point of interest deleted.",
            $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted");
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> UpdatePointOfInterestAsync(
        int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return false;
        }

        var pointOfInterestEntity = await repository.GetPointOfInterestForCityAsync(
            cityId, pointOfInterestId, cancellationToken);
        if (pointOfInterestEntity is null)
        {
            return false;
        }

        mapper.Map(pointOfInterest, pointOfInterestEntity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<int?> BulkUpdatePointsOfInterestDescriptionAsync(
        int cityId,
        string newDescription,
        CancellationToken cancellationToken)
    {
        if (!await repository.CityExistsAsync(cityId, cancellationToken))
        {
            return null;
        }

        return await repository.UpdatePointsPointOfInterestDescriptionForCityAsync(
            cityId, newDescription, cancellationToken);
    }

    private void SendCreationNotification(int cityId, PointOfInterest pointOfInterest)
    {
        var subject = "New Point of Interest Added";
        var message = $"A new point of interest '{pointOfInterest.Name} has been added to city {cityId}'";
        mailService.Send(subject, message);
    }
}