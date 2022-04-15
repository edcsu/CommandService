using CommandService.Business.Entities;
using CommandService.Business.Repositories.Interfaces;
using CommandService.Data;

namespace CommandService.Business.Repositories.Implementations
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly ApplicationDbContext _context;

        public PlatformRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void CreatePlatform(Platform platform)
        {
           await _context.Platforms.AddAsync(platform);
        }

        public bool DoesPlatformExist(Guid platformId)
        {
           return _context.Platforms.Any(p => p.Id == platformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdatePlatform(Platform platform)
        {
            _context.Platforms.Update(platform);
        }
    }
}
