using BeerShared.Data;
using BeerShared.DTO;
using BeerShared.Interfaces;
using BeerShared.Queries;
using Microsoft.EntityFrameworkCore;
using ShopBeerService.Converters;
using ShopBeerService.Infrastructure;
using ShopParsers;
using System.Text.RegularExpressions;

namespace ShopBeerService.Services
{
    public class BeerService : IBeerShopService
    {
        public BeerService(ShopBeerPGDbContext context)
        {
            this.context = context;
            beers = context.ShopBeers;
        }
        private readonly ShopBeerPGDbContext context;
        private readonly DbSet<ShopBeer> beers;
        public async Task<ApiResult<ShopBeerInfo>> GetShopBeers(ShopBeerQuery shopBeerQuery)
        {
            IQueryable<ShopBeer> query = beers.AsNoTracking();
            if (shopBeerQuery.Name is not null)
            {
                var nameSplited = shopBeerQuery.Name.Split(' ');
                var regexPattern = $"{string.Join(@".*", nameSplited.Select(s => $@"(\y{s}\y)"))}";
                query = query.Where(c => Regex.IsMatch(c.Name, regexPattern, RegexOptions.IgnoreCase));
            }
            if (shopBeerQuery.PriceMax.HasValue)
                query = query.Where(c => c.Price < shopBeerQuery.PriceMax.Value);
            if (shopBeerQuery.PriceMin.HasValue)
                query = query.Where(c => c.Price > shopBeerQuery.PriceMin.Value);
            if (shopBeerQuery.IsAvalible.HasValue)
                query = query.Where(c => c.IsAvailable == shopBeerQuery.IsAvalible.Value);
            if (shopBeerQuery.ShopIds is not null && shopBeerQuery.ShopIds.Any())
                query = query.Where(c => shopBeerQuery.ShopIds.Contains(c.ShopId));
            if (shopBeerQuery.HasSourceBeer.HasValue)
                query = query.Where(c => shopBeerQuery.HasSourceBeer.Value ? c.SourceBeerId != null : c.SourceBeerId == null);
            var count = await query.CountAsync();
            return new ApiResult<ShopBeerInfo>(await query.
                OrderBy(c => c.Name).
                Skip(shopBeerQuery.PageIndex * shopBeerQuery.PageSize).
                Take(shopBeerQuery.PageSize).
                Select(b => BeerInfoConverter.ConvertToShopBeerInfo(b)).
                ToListAsync(),
                count, shopBeerQuery.PageIndex, shopBeerQuery.PageSize);
        }
        public async Task<bool> AddBeer(ShopBeerInfo beer)
        {
            if (beer.Id != 0)
                return false;
            await beers.AddAsync(BeerInfoConverter.ConvertToShopBeer(beer));
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddBeers(IEnumerable<ShopBeerInfo> beerCollection)
        {
            var beersConverted = beerCollection.Where(c => c.Id == 0)
                .Select(b => BeerInfoConverter.ConvertToShopBeer(b));
            await beers.AddRangeAsync(beersConverted);
            await context.SaveChangesAsync();
            return beersConverted.Any();
        }
        public async Task UpdateBeer(ShopBeerInfo beer)
        {
            var shopBeer = BeerInfoConverter.ConvertToShopBeer(beer);
            context.Entry(shopBeer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteBeer(int id)
        {
            var beer = await beers.FirstOrDefaultAsync(c => c.Id == id);
            if (beer is not null)
            {
                beers.Remove(beer);
                await context.SaveChangesAsync();
            }
        }
    }
}
