using Microsoft.EntityFrameworkCore;
namespace ShopBeerService.Infrastructure
{
    public static class ServiceInfrastructureCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContextFactory<ShopBeerPGDbContext>(o=>o.UseNpgsql(configuration.GetConnectionString("PGConnectionString")));
            return serviceCollection;
        }
    }
}
