using CommandService.Business.Entities;
using CommandService.Business.ViewModels;

namespace CommandService.Business.Services
{
    public interface ICommandService
    {
        bool DoesPlatformExist(Guid platformId);

        IEnumerable<Command> GetAllCommandsForPlatform(Guid platformId);

        Command? GetCommand(Guid platformId, Guid commandId);

        Task<PlatformDetailsDto> CreateCommandAsync(Guid platformId, Command command);

        IEnumerable<PlatformDetailsDto> GetAllPlatformsAsync();
    }
}
