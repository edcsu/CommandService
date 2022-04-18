using AutoMapper;
using CommandService.Business.Entities;
using CommandService.Business.ViewModels;
using PlatformService.Business.Platform.Protos;

namespace CommandService.Business.MapperProfiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformDetailsDto>();

            CreateMap<CommandCreateDto, Command>();
            
            CreateMap<CommandUpdateDto, Command>();
            
            CreateMap<Command, CommandCreateDto>();
            
            CreateMap<Command, CommandDetailsDto>();

            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalID, options => options.MapFrom(src => src.Id));

            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalID, options => options.MapFrom(src => Guid.Parse(src.PlatformId)));
        }
    }
}
