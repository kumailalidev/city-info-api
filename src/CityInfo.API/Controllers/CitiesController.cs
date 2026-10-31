using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")] // or "api/[controller]"
public class CitiesController : ControllerBase
{
    [HttpGet()]
    public JsonResult GetCities()
    {
        return new JsonResult(
            new[]
            {
                new {id = 1, Name = "Karachi"},
                new {id = 2, Name = "Lahore"}
            });
    }
}