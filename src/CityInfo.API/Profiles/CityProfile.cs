using AutoMapper;

namespace CityInfo.API.Profiles;

/// <summary>
/// Defines the AutoMapper mappings for city-related DTOs.
/// </summary>
public class CityProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CityProfile"/> class.
    /// </summary>
    public CityProfile()
    {
        CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
        CreateMap<Entities.City, Models.CityDto>();
    }
}