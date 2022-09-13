using BeerEncyclopedia.Domain;
using BeerEncyclopedia.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BeerEncyclopedia.Infrastructure.Data
{
    public class BeerEncyclopediaPgDbContext : BeerEncyclopediaDbContext
    {
        public BeerEncyclopediaPgDbContext(DbContextOptions<BeerEncyclopediaPgDbContext> options) : base(options)
        {

        }
        public override DbSet<Beer> Beers => Set<Beer>();
        public override DbSet<Color> Colors => Set<Color>();
        public override DbSet<Country> Countries => Set<Country>();
        public override DbSet<Style> Styles => Set<Style>();
        public override DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new BeerConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new StyleConfiguration());
            modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
        }
    }
}
