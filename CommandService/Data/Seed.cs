using CommandService.Business.Entities;
using CommandService.Business.Repositories.Interfaces;
using CommandService.SyncDataServices.Grpc;
using Serilog;

namespace CommandService.Data
{
    public static class Seed
    {
        public static void PopulateDb(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient!.ReturnAllPlatforms();
            SeedData(serviceScope.ServiceProvider.GetService<IPlatformRepository>()!, platforms);
        }

        private static void SeedData(IPlatformRepository platformRepository, IEnumerable<Platform>? platforms)
        {
            Log.Information("Seeding data to Database");

            if (platforms is null)
            {
                Log.Information("There are no platforms to seed to the Database");
                return;
            }

            foreach (var platform in platforms)
            {
                if (!platformRepository.DoesExternalPlatformExist(platform.ExternalID))
                {
                    platform.Created = DateTime.UtcNow;
                    platformRepository.CreatePlatform(platform);
                }
                platformRepository.SaveChangesAsync();
            }
            Log.Information("Seeded data to the Database");
        }
    }
}
