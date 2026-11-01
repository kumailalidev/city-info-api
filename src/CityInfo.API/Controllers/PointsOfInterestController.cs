using CityInfo.API.Models;

using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current
            .Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current
            .Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var pointOfInterest = city.PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(pointOfInterest);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId,
        PointOfInterestForCreationDto pointOfInterest) // PointOfInterestForCreationDto is a complex type, [FromBody] attribute is not required
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        // calculating max id for point of interest
        var maxPointOfInterestId = CitiesDataStore.Current.Cities
            .SelectMany(c => c.PointsOfInterest)
            .Max(p => p.Id);

        var finalPointOfInterest = new PointOfInterestDto()
        {
            Id = ++maxPointOfInterestId,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };

        city.PointsOfInterest.Add(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest",
         new
         {
             cityId,
             pointOfInterestId = finalPointOfInterest.Id
         },
         finalPointOfInterest);
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current
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

        return NoContent();
    }

    // Full update
    [HttpPut("{pointOfInterestId}")]
    public ActionResult UpdatePointOfInterest(int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest)
    {
        var city = CitiesDataStore.Current
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
        var city = CitiesDataStore.Current
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