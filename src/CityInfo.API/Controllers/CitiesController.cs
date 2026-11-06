using System.Text.Json;

using Asp.Versioning;

using AutoMapper;

using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/v{version:apiVersion}/cities")] // or "api/v{version:apiVersion}/[controller]"
[ApiController]
[ApiVersion(1)] // default
public class CitiesController(ICityInfoRepository cityInfoRepository,
    IMapper mapper) : ControllerBase
{
    const int _maxCitiesPageSize = 20;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery(Name = "name")] string? name,
        [FromQuery(Name = "searchQuery")] string? searchQuery,
        [FromQuery(Name = "pageNumber")] int pageNumber = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 10,
        CancellationToken cancellationToken = default) // [FromQuery] attribute is optional; [ApiController] takes care of it.
    {
        if (pageSize > _maxCitiesPageSize)
        {
            pageSize = _maxCitiesPageSize;
        }

        var (cityEntities, paginationMetadata) = await cityInfoRepository.GetCitiesReadOnlyAsync(
            name,
            searchQuery,
            pageNumber,
            pageSize,
            cancellationToken); // Only read the data, do not track the changes (to object)

        if (paginationMetadata != null)
        {
            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));
        }

        return Ok(mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    /// <summary>
    /// Get a city by id
    /// </summary>
    /// <param name="cityId">The id of the city to get</param>
    /// <param name="includePointsOfInterest">Whether of not to include the point of interest</param>
    /// <param name="cancellationToken">The injected cancellation token</param>
    /// <returns>A city with of without points of interest</returns>
    [HttpGet("{cityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // If request is malformed; 400 is returned; [ApiController] adds this behavior 
    public async Task<IActionResult> GetCity(int cityId, bool includePointsOfInterest = false, CancellationToken cancellationToken = default)
    {
        var city = await cityInfoRepository
            .GetCityAsync(cityId, includePointsOfInterest);

        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(mapper.Map<CityDto>(city));
        }

        return Ok(mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}