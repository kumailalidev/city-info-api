using AutoMapper;

using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")] // or "api/[controller]"
public class CitiesController(ICityInfoRepository cityInfoRepository,
    IMapper mapper) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery(Name = "name")] string? name,
        [FromQuery(Name = "searchQuery")] string? searchQuery,
        CancellationToken cancellationToken = default) // [FromQuery] attribute is optional; [ApiController] takes care of it.
    {
        var cityEntities = await cityInfoRepository.GetCitiesReadOnlyAsync(
            name,
            searchQuery,
            cancellationToken); // Only read the data, do not track the changes (to object)

        return Ok(mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        var city = await cityInfoRepository
            .GetCityAsync(id, includePointsOfInterest);

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