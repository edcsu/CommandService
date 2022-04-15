using CommandService.Business.Entities;

namespace CommandService.Business.Repositories.Interfaces
{
    public interface ICommandRepository : IBaseRepository
    {
        IEnumerable<Command> GetAllCommandsForPlatform(Guid platformId);

        Command? GetCommand(Guid platformId, Guid commandId);

        void CreateCommand(Guid platformId, Command command);

        void UpdateCommand(Guid platformId, Guid commandId, Command command);
    }
}
