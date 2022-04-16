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

        public async Task<CommandDetailsDto?> CreateCommandAsync(Guid platformId, CommandCreateDto commandCreateDto)
        {
            if (!_platformRepository.DoesPlatformExist(platformId))
            {
                return null;
            }

            var command = _mapper.Map<Command>(commandCreateDto);
            command.Created = DateTime.UtcNow;

            _commandRepository.CreateCommand(platformId, command);
            await _commandRepository.SaveChangesAsync();

            return _mapper.Map<CommandDetailsDto>(command);
        }

        public IEnumerable<CommandDetailsDto>? GetAllCommandsForPlatform(Guid platformId)
        {
            if (!_platformRepository.DoesPlatformExist(platformId))
            {
                return null;
            }

            var commands = _commandRepository.GetAllCommandsForPlatform(platformId);
            return _mapper.Map<IEnumerable<CommandDetailsDto>>(commands);
        }

        public IEnumerable<PlatformDetailsDto> GetAllPlatformsAsync()
        {
            var platforms =  _platformRepository.GetAllPlatforms();
            return _mapper.Map<IEnumerable<PlatformDetailsDto>>(platforms);
        }

        public CommandDetailsDto? GetCommand(Guid platformId, Guid commandId)
        {
            if (!_platformRepository.DoesPlatformExist(platformId))
            {
                return null;
            }

            var command = _commandRepository.GetCommand(platformId, commandId);

            if (command is null)
            {
                return null;
            }

            return _mapper.Map<CommandDetailsDto>(command);
        }
    }
}
