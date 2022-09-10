using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopBeerService.Services;
using ShopParsers.Http.ProxyParsers;
using ShopParsers.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ShopBeerService.Workers
{
    public static class WorkersServiceExtension
    {
        public static IServiceCollection ConfigureWorkers(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IWebDriverService>(b => new ChromeDriverService(
                configuration.GetSection("ParsersConfiguration")["WebDriverPath"]));
            serviceCollection.AddTransient<ProxyParser, FoxtoolsProxyParser>();
            serviceCollection.AddTransient<ProxyParser, HidemyProxyParser>(c => new HidemyProxyParser(
                configuration.GetSection("ParsersConfiguration")["HidemyCookie"],
                c.GetRequiredService<ILogger<HidemyProxyParser>>()));
            serviceCollection.AddTransient<IWebProxyService, WebProxyService>();
            
            serviceCollection.AddHostedService(
                b => new MetroParserService(new BeerShopServiceArgs(new StartTimeServiceArgs(DayOfWeek.Monday, 24), 4, "Метро"),
                b.GetRequiredService<IDbContextFactory<ShopBeerPGDbContext>>(),
                b.GetRequiredService<IWebDriverService>(),
                b.GetRequiredService<ILogger<MetroParserService>>()));

            serviceCollection.AddHostedService(c =>
            {
                return new PerekrestokParserService(
                     new BeerShopServiceArgs(new StartTimeServiceArgs(DayOfWeek.Wednesday, 24), 1, "Перекресток"),
                     c.GetRequiredService<IWebProxyService>(),
                     false,
                     c.GetRequiredService<IDbContextFactory<ShopBeerPGDbContext>>(),
                     c.GetRequiredService<ILogger<PerekrestokParserService>>());
            });
            serviceCollection.AddHostedService(c =>
            {
                return new WineStyleParserService(
                     new BeerShopServiceArgs(new StartTimeServiceArgs(DayOfWeek.Saturday, 24), 5, "WineStyle"),
                     c.GetRequiredService<IDbContextFactory<ShopBeerPGDbContext>>(),
                     c.GetRequiredService<ILogger<WineStyleParserService>>(), c.GetRequiredService<IWebProxyService>(),
                     false);
            });
            serviceCollection.AddHostedService(c =>
            {
                return new LentaParserService(
                     new BeerShopServiceArgs(new StartTimeServiceArgs(DayOfWeek.Friday, 24), 1, "Лента"),
                     c.GetRequiredService<IDbContextFactory<ShopBeerPGDbContext>>(),
                     c.GetRequiredService<ILogger<LentaParserService>>(), c.GetRequiredService<IWebProxyService>(),
                     false);
            });
            return serviceCollection;
        }
    }
}
