using AutoMapper;

using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles;

/// <summary>
/// Maps point-of-interest entities and DTOs for the API.
/// </summary>
public class PointOfInterestProfile : Profile
{
    /// <summary>
    /// Initializes the point-of-interest mapping configuration.
    /// </summary>
    public PointOfInterestProfile()
    {
        CreateMap<PointOfInterest, PointOfInterestDto>();
        CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
        CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
        CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
    }
}