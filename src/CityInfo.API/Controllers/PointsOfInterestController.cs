using Asp.Versioning;

using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

/// <summary>
/// Handles requests related to points of interest for a city.
/// </summary>
/// <param name="logger">The logger used to record controller activity.</param>
/// <param name="pointOfInterestService">The service used to manage point-of-interest operations.</param>
[Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
[ApiController]
[ApiVersion(1)] // default
public class PointsOfInterestController(
    ILogger<PointsOfInterestController> logger,
    IPointOfInterestService pointOfInterestService) : ControllerBase
{
    /// <summary>
    /// Gets all points of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of points of interest if the city exists; otherwise, <see cref="NotFoundResult"/>.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
        "Getting points of interest for city with id {cityId}", cityId);

        var pointsOfInterest = await pointOfInterestService
            .GetPointsOfInterestForCityAsync(cityId, cancellationToken);
        if (pointsOfInterest is null)
        {
            logger.LogInformation(
            "City with id {cityId} wasn't found when accessing points of interest", cityId);
            return NotFound();
        }

        return Ok(pointsOfInterest);
    }

    /// <summary>
    /// Gets a single point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point of interest identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The point of interest if it exists; otherwise, <see cref="NotFoundResult"/>.</returns>
    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId,
        CancellationToken cancellationToken = default)
    {
        var pointOfInterest = await pointOfInterestService
            .GetPointOfInterestForCityAsync(cityId, pointOfInterestId, cancellationToken);
        if (pointOfInterest is null)
        {
            logger.LogInformation(
                "City or point of interest wasn't found for city id {cityId} and point-of-interest id {pointOfInterestId}",
                cityId,
                pointOfInterestId);
            return NotFound();
        }

        return Ok(pointOfInterest);
    }

    /// <summary>
    /// Creates a new point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterest">The point of interest data to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created point of interest.</returns>
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

    /// <summary>
    /// Deletes a point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point of interest identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the outcome of the delete operation.</returns>
    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId,
        CancellationToken cancellationToken = default)
    {
        return await pointOfInterestService.DeletePointOfInterestAsync(
            cityId, pointOfInterestId, cancellationToken)
            ? NoContent()
            : NotFound();
    }

    /// <summary>
    /// Replaces the contents of a point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point of interest identifier.</param>
    /// <param name="pointOfInterest">The updated point of interest data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the outcome of the update operation.</returns>
    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId,
        int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest,
        CancellationToken cancellationToken = default)
    {
        if (!await pointOfInterestService.UpdatePointOfInterestAsync(
            cityId, pointOfInterestId, pointOfInterest, cancellationToken))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Applies a JSON Patch document to a point of interest for the specified city.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="pointOfInterestId">The point of interest identifier.</param>
    /// <param name="patchDocument">The JSON Patch document describing the partial update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ActionResult"/> indicating the outcome of the patch operation.</returns>
    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument,
        CancellationToken cancellationToken = default)
    {
        var pointOfInterestToPatch = await pointOfInterestService
            .GetPointOfInterestForUpdateAsync(cityId, pointOfInterestId, cancellationToken);
        if (pointOfInterestToPatch is null)
        {
            return NotFound();
        }

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

        if (!await pointOfInterestService.UpdatePointOfInterestAsync(
            cityId, pointOfInterestId, pointOfInterestToPatch, cancellationToken))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Updates the description for all points of interest in the specified city.
    /// </summary>
    /// <param name="cityId">The city whose points of interest should be updated.</param>
    /// <param name="pointsOfInterestBulkUpdateDto">The bulk update payload containing the new description.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of updated records.</returns>
    [HttpPut("bulk")]
    public async Task<ActionResult> BulkUpdatePointsOfInterestDescription(int cityId,
        PointsOfInterestBulkUpdateDto pointsOfInterestBulkUpdateDto,
        CancellationToken cancellationToken = default)
    {
        var affectedRows = await pointOfInterestService
            .BulkUpdatePointsOfInterestDescriptionAsync(
                cityId,
                pointsOfInterestBulkUpdateDto.NewDescription,
                cancellationToken);
        if (affectedRows is null)
        {
            return NotFound();
        }

        return Ok(new { UpdateCount = affectedRows });
    }

}