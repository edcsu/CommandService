using AutoMapper;
using CommandService.Business.Entities;
using CommandService.Business.Repositories.Interfaces;
using CommandService.Business.ViewModels;

namespace CommandService.Business.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly ILogger<CommandService> _logger;
        private readonly IMapper _mapper;

        public CommandService(ICommandRepository commandRepository,
            IPlatformRepository platformRepository,
            ILogger<CommandService> logger, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _platformRepository = platformRepository;
            _logger = logger;
            _mapper = mapper;
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
            var platforms =  _platformRepository.GetAllPlatforms();
            return _mapper.Map<IEnumerable<PlatformDetailsDto>>(platforms);
        }

        public Command? GetCommand(Guid platformId, Guid commandId)
        {
            throw new NotImplementedException();
        }
    }
}
