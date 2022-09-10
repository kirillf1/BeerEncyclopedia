using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopBeerService.Services;
using ShopParsers;
using ShopParsers.Lenta;
using System.Net;

namespace ShopBeerService.Workers
{
    public class LentaParserService : BeerShopParserService
    {
        public LentaParserService(BeerShopServiceArgs beerShopServiceArgs,
            IDbContextFactory<ShopBeerPGDbContext> contextFactory,
            ILogger logger, IWebProxyService webProxyService, bool canParseWithoutProxy) : base(beerShopServiceArgs, contextFactory, logger)
        {
            this.webProxyService = webProxyService;
            this.canParseWithoutProxy = canParseWithoutProxy;
        }
        private readonly IWebProxyService webProxyService;
        private readonly bool canParseWithoutProxy;

        protected async override Task<IEnumerable<ShopBeer>> ParseBeers()
        {
            var threadsCount = beerShopServiceArgs.Threads <= 0 ? 1 : beerShopServiceArgs.Threads;
            var lentaBeerCategories = LentaBeerParser.GetLentaBeerCategories();
            var tasks = new List<Task<IEnumerable<ShopBeer>>>(threadsCount);
            var beers = new List<ShopBeer>();
            foreach (var category in lentaBeerCategories)
            {
                tasks.Add(GetBeersByCategory(category));
                if(tasks.Count >= threadsCount)
                {
                    await Task.WhenAll(tasks);
                    beers.AddRange(tasks.Where(c => c.IsCompletedSuccessfully).SelectMany(s => s.Result));
                    tasks.Clear();
                }                    
            }
            return beers;
        }
        private async Task<IEnumerable<ShopBeer>> GetBeersByCategory(LentaBeerCategory lentaBeerCategory)
        {
            var random = new Random();
            var tryCount = 45;
            int maxPageCount = 2;
            var currentPage = 1;
            var beers = new List<ShopBeer>();
            var proxies = new List<IWebProxy>(await webProxyService.GetProxiesByCountry(tryCount, "RU"));
            if (canParseWithoutProxy)
                proxies.Add(new WebProxy());
            foreach (var proxy in proxies)
            {
                try
                {
                    using var parser = new LentaBeerParser(proxy);
                    while (maxPageCount > currentPage)
                    {
                        beers.AddRange(await parser.ParseBeers(lentaBeerCategory, currentPage));
                        maxPageCount = await parser.GetPageCount(lentaBeerCategory);
                        currentPage++;
                        await Task.Delay(random.Next(1000, 5000));

                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Can't parse beer page {page}, category: {category}. Error message: {message}", 
                        currentPage,lentaBeerCategory,ex.Message);
                }

            }
            return beers;
        }
    }
}
