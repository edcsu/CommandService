using AutoMapper;
using CommandService.Business.Entities;
using CommandService.Business.Repositories.Interfaces;
using CommandService.Business.ViewModels;
using CommandService.Core;
using System.Text.Json;

namespace CommandService.Business.EventProcessor
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory,
            IMapper mapper, ILogger<EventProcessor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public void ProcessEvent(string message)
        {
            var eventfound = DetermineEvent(message);

            switch (eventfound)
            {
                case EventType.PlatFormPublished:
                    AddPlatform(message);
                    break;

                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation("Determining notification event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            if (eventType is null)
            {
                _logger.LogInformation("Event couldnot be detected");
                return EventType.Undetermined;
            }

            switch (eventType.Event)
            {
                case PlatformEvents.PlatFormPublished:
                     _logger.LogInformation("PlatForm published event detected");
                    return EventType.PlatFormPublished;

                default:
                    _logger.LogInformation("Event couldnot be detected");
                    return EventType.Undetermined;
            }
        }

        private void AddPlatform(string publishedMessage)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();

            var publishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(publishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(publishedDto);
                if (!repo.DoesExternalPlatformExist(platform.ExternalID))
                {
                    repo.CreatePlatform(platform);
                    repo.SaveChangesAsync().Wait();
                }
                else
                {
                    _logger.LogError("Platform already exists with externalID: {ExternalID}", platform.ExternalID);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldnot add platform to DB");
                //throw;
            }
        }
    }
}