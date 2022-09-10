using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopBeerService.Services;
using ShopParsers;
using ShopParsers.Metro;

namespace ShopBeerService.Workers
{
    public class MetroParserService : BeerShopParserService
    {
        public MetroParserService(BeerShopServiceArgs beerShopServiceArgs, IDbContextFactory<ShopBeerPGDbContext> contextFactory,
            IWebDriverService webDriverService, ILogger<MetroParserService> logger) 
            : base(beerShopServiceArgs, contextFactory,logger)
        {
            this.webDriverService = webDriverService;
        }
        private readonly IWebDriverService webDriverService;
        protected override async Task<IEnumerable<ShopBeer>> ParseBeers()
        {
            var webDriversCount = beerShopServiceArgs.Threads;
            using var driversContainer = webDriverService.GetConfiguredWebDrivers(webDriversCount);
            var drivers = driversContainer.WebDrivers;
            var parser = new MetroBeerParser(drivers.First(), logger);
            var totalPages = parser.GetTotalPages();
            var pageParts = GetEqualPageParts(totalPages, webDriversCount).ToArray();
            var parsers = new List<MetroBeerParser>(pageParts.Length)
            {
                parser
            };
            foreach (var driver in drivers.Skip(1))
            {
                parsers.Add(new MetroBeerParser(driver, logger));
            }
            if (pageParts.Length != parsers.Count)
                return await parser.ParseBeers();
            var tasks = new List<Task<IEnumerable<ShopBeer>>>(parsers.Count);
            int startPage = 1;
            for (int i = 0; i < pageParts.Length; i++)
            {
                var endPage = startPage + pageParts[i];
                tasks.Add(parsers[i].ParseBeers(startPage, endPage));
                logger.LogInformation("Metro parser started parsing at {date}. Page range: {start}-{end}",DateTime.Now.ToString(),startPage,endPage);
                startPage = endPage;
            }
            await Task.WhenAll(tasks);
            return tasks.Where(t => t.IsCompletedSuccessfully).Select(t => t.Result).SelectMany(c => c);
        }
    }
}
