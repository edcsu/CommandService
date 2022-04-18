using AutoMapper;
using CommandService.Business.Config;
using CommandService.Business.Entities;
using Grpc.Net.Client;
using PlatformService.Business.Platform.Protos;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PlatformDataClient> _logger;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, 
            IMapper mapper, 
            ILogger<PlatformDataClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<Platform>? ReturnAllPlatforms()
        {
            var grpcConfing = _configuration.GetGrpcPlatformConfig();
            _logger.LogInformation("Calling GRPC Service: {GrpcService}", grpcConfing.Url);
        
            var channel = GrpcChannel.ForAddress(grpcConfing.Url);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Couldnot call GPRC server");
                //throw;
                return null;
            }
        }
    }
}
