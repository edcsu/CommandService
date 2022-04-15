using CommandService.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
#nullable disable
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)   
        {
        }

        public DbSet<Platform>  Platforms { get; set; }
        public DbSet<Command>  Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasIndex(p => p.Id)
                .IsUnique();
            
            modelBuilder
                .Entity<Platform>()
                .HasIndex(p => p.Name);
            
            modelBuilder
                .Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Platform)
                .HasForeignKey(p => p.PlatformID);

            modelBuilder
                .Entity<Command>()
                .HasIndex(c => c.Id)
                .IsUnique();

            modelBuilder
                .Entity<Command>()
                .HasOne(c => c.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(c => c.PlatformID);
        }
    }
}
