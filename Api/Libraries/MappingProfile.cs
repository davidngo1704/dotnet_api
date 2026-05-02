using Api.Database.Models;
using Api.Models.ApplicationModels;
using AutoMapper;

namespace Api.Libraries;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Human, HumanEditModel>();
        CreateMap<HumanEditModel, Human>();
    }
}