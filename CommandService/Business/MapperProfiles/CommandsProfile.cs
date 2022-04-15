using AutoMapper;
using CommandService.Business.Entities;
using CommandService.Business.ViewModels;

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
        }
    }
}
