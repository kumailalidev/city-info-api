using CityInfo.API.Models;
using CityInfo.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")] // or "api/[controller]"
public class CitiesController(ICityInfoRepository cityInfoRepository, CitiesDataStore citiesDataStore) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cityEntities = await cityInfoRepository.GetCitiesReadOnlyAsync(); // Only read the data, do not track the changes (to object)

        var results = new List<CityWithoutPointsOfInterestDto>();
        foreach (var cityEntity in cityEntities)
        {
            results.Add(new CityWithoutPointsOfInterestDto
            {
                Id = cityEntity.Id,
                Name = cityEntity.Name,
                Description = cityEntity.Description
            });
        }

        return Ok(results);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var cityToReturn = citiesDataStore
            .Cities.FirstOrDefault(c => c.Id == id);

        if (cityToReturn == null)
        {
            return NotFound();
        }

        return Ok(cityToReturn);
    }
}