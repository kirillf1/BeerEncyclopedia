using BeerShared.Data;
using BeerShared.DTO;
using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;
using ShopBeerService.Queries;
using ShopParsers;

namespace ShopBeerService.Services
{
    public class BeerService
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
                query = query.Where(c => EF.Functions.ILike(c.Name, $"%{shopBeerQuery.Name}%"));
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
                Select(b => ShopBeerToInfo(b)).
                Skip(shopBeerQuery.PageIndex * shopBeerQuery.PageSize).
                Take(shopBeerQuery.PageSize).
                OrderBy(c => c.Name).ToListAsync(),
                count, shopBeerQuery.PageIndex, shopBeerQuery.PageSize);
        }
        public async Task AddBeer(ShopBeer beer)
        {
            await beers.AddAsync(beer);
            await context.SaveChangesAsync();
        }
        public async Task AddBeers(IEnumerable<ShopBeer> beerCollection)
        {
            await beers.AddRangeAsync(beerCollection);
            await context.SaveChangesAsync();
        }
        public async Task UpdateBeer(ShopBeer shopBeer)
        {
            context.Entry(shopBeer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteBeer(Guid? shopId, string name)
        {
            var beer = await beers.FirstOrDefaultAsync(c => c.ShopId == shopId && c.Name == name);
            if (beer is not null)
            {
                beers.Remove(beer);
                await context.SaveChangesAsync();
            }
        }
        public static ShopBeerInfo ShopBeerToInfo(ShopBeer beer)
        {
            return new ShopBeerInfo
            {
                Brand = beer.Brand,
                Description = beer.Description,
                ShopId = beer.ShopId,
                SourceBeerId = beer.SourceBeerId,
                Strength = beer.Strength,
                Style = beer.Style,
                Color = beer.Color,
                Country = beer.Country,
                DiscountPrice = beer.DiscountPrice,
                Filtration = beer.Filtration,
                InitialWort = beer.InitialWort,
                Manufacturer = beer.Manufacturer,
                Name = beer.Name,
                NameEn = beer.NameEn,
                Pasteurization = beer.Pasteurization,
                Price = beer.Price,
                Rating = beer.Rating,
                Volume = beer.Volume
            };
        }
    }
}
