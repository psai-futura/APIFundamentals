using AutoMapper;

namespace APIFundamentals.Profiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointsOfInterestDto>();
        CreateMap<Models.PointOfInterestCreateDto, Entities.PointOfInterest>();
        CreateMap<Models.PointOfInterestUpdateDto, Entities.PointOfInterest>();
        CreateMap<Entities.PointOfInterest,Models.PointOfInterestUpdateDto>();
    }
    
}