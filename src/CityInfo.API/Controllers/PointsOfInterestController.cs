using AutoMapper;

using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController(
    CitiesDataStore citiesDataStore,
    ICityInfoRepository cityInfoRepository,
    IMapper mapper,
    ILogger<PointsOfInterestController> logger,
    IMailService mailService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            logger.LogInformation(
            "City with id {cityId} wasn't found when accessing points of interest", cityId);
            return NotFound();
        }

        var pointsOfInterestForCity = await cityInfoRepository
            .GetPointsOfInterestForCityAsync(cityId);

        return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            logger.LogInformation(
            "City with id {cityId} wasn't found when accessing points of interest", cityId);
            return NotFound();
        }

        var pointOfInterest = await cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
        PointOfInterestForCreationDto pointOfInterest) // PointOfInterestForCreationDto is a complex type, [FromBody] attribute is not required
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            logger.LogInformation(
            "City with id {cityId} wasn't found when accessing points of interest", cityId);
            return NotFound();
        }

        var finalPointOfInterest = mapper.Map<PointOfInterest>(pointOfInterest);

        await cityInfoRepository
            .AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
        await cityInfoRepository.SaveChangesAsync();

        var createdPointOfInterestedToReturn = mapper.Map<PointOfInterestDto>(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest",
         new
         {
             cityId,
             pointOfInterestId = createdPointOfInterestedToReturn.Id
         },
         createdPointOfInterestedToReturn);
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = citiesDataStore
            .Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        city.PointsOfInterest.Remove(pointOfInterestFromStore);

        mailService.Send("Point of interest deleted.",
            $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");

        return NoContent();
    }

    // Full update
    [HttpPut("{pointOfInterestId}")]
    public ActionResult UpdatePointOfInterest(int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest)
    {
        var city = citiesDataStore
            .Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        // Fully update the resource; If consumer does not provide a value for a field, set as default value
        pointOfInterestFromStore.Name = pointOfInterest.Name;
        pointOfInterestFromStore.Description = pointOfInterest.Description;

        return NoContent();
    }

    // Partial update
    [HttpPatch("{pointOfInterestId}")]
    public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        var city = citiesDataStore
            .Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterestFromStore = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (pointOfInterestFromStore == null)
        {
            return NotFound();
        }

        // Transforming to PointOfInterestForUpdateDto obj.
        var pointOfInterestToPatch =
            new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

        patchDocument.ApplyTo(pointOfInterestToPatch, jsonPatchError =>
        {
            // making model state invalid
            var key = jsonPatchError.AffectedObject.GetType().Name;
            ModelState.AddModelError(key, jsonPatchError.ErrorMessage);
        });

        if (!ModelState.IsValid) // Validating JsonDocument model
        {
            return BadRequest(ModelState);
        }

        if (!TryValidateModel(pointOfInterestToPatch)) // Validating pointOfInterestToPatch model; required for validating data annotations
        {
            return BadRequest(ModelState);
        }

        pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
        return NoContent();
    }
}