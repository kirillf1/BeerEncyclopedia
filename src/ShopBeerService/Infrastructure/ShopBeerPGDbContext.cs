using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure.EntityConfigurations;
using ShopParsers;

namespace ShopBeerService.Infrastructure
{
    public class ShopBeerPGDbContext : DbContext
    {
        public ShopBeerPGDbContext(DbContextOptions<ShopBeerPGDbContext> options) : base(options)
        {

        }
        public DbSet<Shop> Shops => Set<Shop>();
        public DbSet<ShopBeer> ShopBeers => Set<ShopBeer>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new ShopBeerConfiguration());
            modelBuilder.ApplyConfiguration(new ShopConfiguration());
        }
    }
}
