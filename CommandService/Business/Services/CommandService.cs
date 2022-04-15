using CommandService.Business.Entities;
using CommandService.Business.ViewModels;

namespace CommandService.Business.Services
{
    public class CommandService : ICommandService
    {
        public CommandService()
        {

        }

        public Task<PlatformDetailsDto> CreateCommandAsync(Guid platformId, Command command)
        {
            throw new NotImplementedException();
        }

        public bool DoesPlatformExist(Guid platformId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommandsForPlatform(Guid platformId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PlatformDetailsDto> GetAllPlatformsAsync()
        {
            throw new NotImplementedException();
        }

        public Command? GetCommand(Guid platformId, Guid commandId)
        {
            throw new NotImplementedException();
        }
    }
}
