using AutoMapper;
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
            throw new NotImplementedException();
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation("Determining notification event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            if (eventType is null)
            {
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
    }

}
