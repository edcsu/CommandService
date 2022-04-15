using CommandService.Business.Entities;

namespace CommandService.Business.Repositories.Interfaces
{
    public interface IPlatformRepository : IBaseRepository
    {
        IEnumerable<Platform> GetAllPlatforms();

        bool DoesPlatformExist(Guid platformId);

        bool DoesExternalPlatformExist(Guid externalPlatformId);

        void CreatePlatform(Platform platform);

        void UpdatePlatform(Platform platform);
    }
}
