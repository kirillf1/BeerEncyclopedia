using BeerEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeerEncyclopedia.Infrastructure.Data
{
    public abstract class BeerEncyclopediaDbContext : DbContext
    {
        public BeerEncyclopediaDbContext(DbContextOptions options) : base(options)
        {
        }
        public abstract DbSet<Beer> Beers { get; }
        public abstract DbSet<Color> Colors { get; }
        public abstract DbSet<Country> Countries { get; }
        public abstract DbSet<Style> Styles { get; }
        public abstract DbSet<Manufacturer> Manufacturers { get; }
    }
}
