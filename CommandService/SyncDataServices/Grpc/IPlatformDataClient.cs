using CommandService.Business.Entities;

namespace CommandService.SyncDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform>? ReturnAllPlatforms();
    }
}
