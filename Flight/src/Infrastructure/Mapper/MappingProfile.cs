using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FlightDto, Flight>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}
