using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopParsers;

namespace ShopBeerService.Workers
{
    public abstract class BeerShopParserService : BackgroundService
    {
        protected BeerShopParserService(BeerShopServiceArgs beerShopServiceArgs, IDbContextFactory<ShopBeerPGDbContext> contextFactory, ILogger logger)
        {
            this.beerShopServiceArgs = beerShopServiceArgs;
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        protected readonly BeerShopServiceArgs beerShopServiceArgs;
        protected readonly IDbContextFactory<ShopBeerPGDbContext> contextFactory;
        protected readonly ILogger logger;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var delayTime = beerShopServiceArgs.StartTimeServiceArgs.GetDelayTime();
                    logger.LogInformation("Shop parser waiting time to parse: {time}", delayTime.ToString());
                    await Task.Delay(delayTime, stoppingToken);
                    var beerResult = await ParseBeers();
                    var beers = beerResult.Distinct().ToList();
                    using var context = await contextFactory.CreateDbContextAsync();
                    var shop = await GetShopWithBeers(context, beerShopServiceArgs.ShopName);
                    if (shop is null)
                    {
                        shop = new Shop(Guid.NewGuid(), beerShopServiceArgs.ShopName, beers);
                        context.Add(shop);
                    }
                    else
                    {
                        var existedBeers = shop.ShopBeers.Join(beers,
                            b => b.Name, b => b.Name,
                            (oldBeer, newBeer) => new { oldBeer, newBeer }).ToArray();
                        foreach (var beer in existedBeers)
                        {
                            TransferChangingBeerProperties(beer.newBeer, beer.oldBeer);
                        }
                        shop.ShopBeers.AddRange(beers.Except(existedBeers.Select(c => c.newBeer)));
                    }
                    await context.SaveChangesAsync(stoppingToken);
                    logger.LogInformation("Parsing ended. Finded {count} beers in shop", beers.Count);
                }
                catch(Exception ex)
                {
                    logger.LogError("Parsing failed with error: {ex}", ex.Message);
                }
            }

        }
        protected abstract Task<IEnumerable<ShopBeer>> ParseBeers();
        protected async Task<Shop?> GetShopWithBeers(string shopName)
        {
            using var context = await contextFactory.CreateDbContextAsync();
            return await GetShopWithBeers(context, shopName);
        }
        private static async Task<Shop?> GetShopWithBeers(ShopBeerPGDbContext context, string shopName)
        {
            return await context.Shops.Include(c => c.ShopBeers).FirstOrDefaultAsync(
               s => s.Name.ToLower() == shopName.ToLower());
        }
        protected static IEnumerable<int> GetEqualPageParts(int total, int divider)
        {
            if (divider == 0)
            {
                yield return 0;
            }
            else
            {
                int rest = total % divider;
                double result = total / (double)divider;

                for (int i = 0; i < divider; i++)
                {
                    if (rest-- > 0)
                        yield return (int)Math.Ceiling(result);
                    else
                        yield return (int)Math.Floor(result);
                }
            }
        }
        protected void TransferChangingBeerProperties(ShopBeer shopBeerNew, ShopBeer shopBeerOld)
        {
            shopBeerOld.Brand = shopBeerNew.Brand;
            shopBeerOld.Manufacturer = shopBeerNew.Manufacturer;
            shopBeerOld.DetailsUrl = shopBeerNew.DetailsUrl;
            shopBeerOld.Price = shopBeerNew.Price;
            shopBeerOld.DiscountPrice = shopBeerNew.DiscountPrice;
            shopBeerOld.IsAvailable = shopBeerNew.IsAvailable;
            shopBeerOld.Rating = shopBeerNew.Rating;
            shopBeerOld.Volume = shopBeerNew.Volume;
        }
    }
}
