using CommandService.Business.Entities;

namespace CommandService.Business.Repositories.Interfaces
{
    public interface IPlatformRepository
    {
        IEnumerable<Platform> GetAllPlatforms();

        bool DoesPlatformexist(Guid platformId);

        void CreatePlatform(Platform platform);

        void UpdatePlatform(Platform platform);
    }
}
