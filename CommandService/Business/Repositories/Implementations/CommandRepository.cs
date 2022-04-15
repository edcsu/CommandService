
using CommandService.Business.Entities;
using CommandService.Business.Repositories.Interfaces;
using CommandService.Data;

namespace CommandService.Business.Repositories.Implementations
{
    public class CommandRepository : IBaseRepository, ICommandRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void CreateCommand(Guid platformId, Command command)
        {
            command.PlatformID = platformId;
            await _context.Commands.AddAsync(command);
        }

        public IEnumerable<Command> GetAllCommandsForPlatform(Guid platformId)
        {
            return _context.Commands.
                Where(c => c.PlatformID == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public Command? GetCommand(Guid platformId, Guid commandId)
        {
            return _context.Commands
                .Where(c => c.PlatformID == platformId && c.Id == commandId)
                .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdatePlatform(Guid platformId, Guid commandId, Command command)
        {
            if (GetCommand(platformId, commandId) is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Commands.Update(command);
        }
    }
}
