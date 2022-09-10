using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopBeerService.Services;
using ShopParsers;
using ShopParsers.Lenta;
using ShopParsers.WineStyle;
using System.Net;

namespace ShopBeerService.Workers
{
    public class WineStyleParserService : BeerShopParserService
    {
        const int pageOffset = 100;
        public WineStyleParserService(BeerShopServiceArgs beerShopServiceArgs,
            IDbContextFactory<ShopBeerPGDbContext> contextFactory,
            ILogger logger, IWebProxyService webProxyService,
            bool canParseWithoutProxy) : base(beerShopServiceArgs, contextFactory, logger)
        {
            this.webProxyService = webProxyService;
            this.canParseWithoutProxy = canParseWithoutProxy;
        }
        private readonly IWebProxyService webProxyService;
        private readonly bool canParseWithoutProxy;
        Random random = new Random();
        protected override async Task<IEnumerable<ShopBeer>> ParseBeers()
        {
            //TODO implement beer Size in site
            var threads = beerShopServiceArgs.Threads;
            var chunks = GetEqualPageParts(5000, threads).ToArray();
            if (chunks.Length != threads)
                threads = 1;
            var tasks = new List<Task<IEnumerable<ShopBeer>>>(threads);
            var start = 0;
            for (int i = 0; i < threads; i++)
            {
                var chunkCount = chunks[i];
                tasks.Add(ParseChunk(start, start + chunkCount));
                start += chunkCount;
            }
            await Task.WhenAll(tasks);
            return tasks.Where(t => t.IsCompletedSuccessfully).SelectMany(b => b.Result);
        }
        private async Task<IEnumerable<ShopBeer>> ParseChunk(int start, int end)
        {
            var beers = new List<ShopBeer>();
            var proxies = new List<IWebProxy>(await webProxyService.GetProxies(50));
            if (canParseWithoutProxy)
                proxies.Add(new WebProxy());
            foreach (var proxy in proxies)
            {
                try
                {
                    using var parser = new WineStyleParser(proxy,logger);
                    while (end > start)
                    {
                        beers.AddRange(await parser.ParseBeers(start, start + pageOffset));
                        start += pageOffset;
                        await Task.Delay(random.Next(1000, 6000));
                    }
                    return beers;
                }
                catch (Exception ex)
                {
                    logger.LogError("Can't parse beer page {page}. Error message: {message}",
                      start, ex.Message);
                }
            }
            return beers;
        }

    }
}
