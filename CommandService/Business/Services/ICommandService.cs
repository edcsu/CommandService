using CommandService.Business.Entities;
using CommandService.Business.ViewModels;

namespace CommandService.Business.Services
{
    public interface ICommandService
    {
        IEnumerable<CommandDetailsDto>? GetAllCommandsForPlatform(Guid platformId);

        CommandDetailsDto? GetCommand(Guid platformId, Guid commandId);

        Task<CommandDetailsDto?> CreateCommandAsync(Guid platformId, CommandCreateDto commandCreateDto);

        IEnumerable<PlatformDetailsDto> GetAllPlatformsAsync();
    }
}
