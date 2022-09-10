using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopBeerService.Services;
using ShopParsers;
using ShopParsers.Perekrestok;
using System.Net;

namespace ShopBeerService.Workers
{
    public class PerekrestokParserService : BeerShopParserService
    {
        public PerekrestokParserService(BeerShopServiceArgs beerShopServiceArgs,
            IWebProxyService webProxyService, bool canParseWithoutProxy,
            IDbContextFactory<ShopBeerPGDbContext> contextFactory, ILogger logger) : base(beerShopServiceArgs, contextFactory, logger)
        {
            this.webProxyService = webProxyService;
            this.canParseWithoutProxy = canParseWithoutProxy;
        }
        private readonly IWebProxyService webProxyService;
        private readonly bool canParseWithoutProxy;

        protected override async Task<IEnumerable<ShopBeer>> ParseBeers()
        {
            //TODO
            //Before parse get shopIds from api and parse of each id
            var proxies = new List<IWebProxy>( await webProxyService.GetProxiesByCountry(100, "RU"));
            if (canParseWithoutProxy)
                proxies.Add(new WebProxy());            
            foreach (var proxy in proxies)
            {
                try
                {
                    using var parser = proxy == null ? new PerekrestokParser() : new PerekrestokParser(proxy);
                    return await parser.ParseBeers();
                }
                catch (Exception ex)
                {
                    logger.LogError("Parsing failed error: {error}", ex.Message);
                }
            }          
            return Enumerable.Empty<ShopBeer>();
        }
    }
}
