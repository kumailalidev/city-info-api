using Asp.Versioning;

using AutoMapper;

using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
[ApiVersion(1)] // default
public class PointsOfInterestController(
    ILogger<PointsOfInterestController> logger,
    ICityInfoRepository cityInfoRepository,
    IPointOfInterestService pointOfInterestService,
    IMapper mapper,
    IMailService mailService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
        "Getting points of interest for city with id {cityId}", cityId);

        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            logger.LogInformation(
            "City with id {cityId} wasn't found when accessing points of interest", cityId);
            return NotFound();
        }

        var pointsOfInterestForCity = await cityInfoRepository
            .GetPointsOfInterestForCityAsync(cityId, cancellationToken);

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
        PointOfInterestForCreationDto pointOfInterest,
        CancellationToken cancellationToken = default) // PointOfInterestForCreationDto is a complex type, [FromBody] attribute is not required
    {
        var result = await pointOfInterestService
            .CreatePointOfInterestAsync(cityId, pointOfInterest, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return CreatedAtRoute("GetPointOfInterest",
         new
         {
             cityId,
             pointOfInterestId = result.PointOfInterest!.Id
         },
         result.PointOfInterest);
    }

    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await cityInfoRepository.SaveChangesAsync();

        mailService.Send("Point of interest deleted.",
            $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

        return NoContent();
    }

    // Full update
    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest)
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        mapper.Map(pointOfInterest, pointOfInterestEntity); // Modifying tracked pointOfInterestEntity with pointOfInterest values
        await cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    // Partial update
    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await cityInfoRepository
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        // Transforming to PointOfInterestForUpdateDto obj.
        var pointOfInterestToPatch = mapper
            .Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

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

        mapper.Map(pointOfInterestToPatch, pointOfInterestEntity); // Modifying tracked pointOfInterestToPatch with pointOfInterestEntity values
        await cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("bulk")]
    public async Task<ActionResult> BulkUpdatePointsOfInterestDescription(int cityId,
        PointsOfInterestBulkUpdateDto pointsOfInterestBulkUpdateDto)
    {
        if (!await cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var affectedRows = await cityInfoRepository
            .UpdatePointsPointOfInterestDescriptionForCityAsync(cityId,
            pointsOfInterestBulkUpdateDto.NewDescription);

        return Ok(new { UpdateCount = affectedRows });
    }

}