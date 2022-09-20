using BeerFormaters.BeerNameFormater;
using BeerShared.DTO;
using BeerShared.Interfaces;
using Cyrillic.Convert;
using Microsoft.EntityFrameworkCore;
using ShopBeerService.Converters;
using ShopBeerService.Infrastructure;
using ShopParsers;

namespace ShopBeerService.Services.SourceBeerServices
{
    public class ShopBeerBinder : IShopBeerBinder
    {
        public ShopBeerBinder(ShopBeerPGDbContext context)
        {
            this.context = context;
        }
        private readonly ShopBeerPGDbContext context;
        public async Task BindBeers(BindedSourceBeer bind, CancellationToken cancellationToken)
        {
            var sourceBeerId = bind.SourceBeerId;
            var shopBeers = await context.ShopBeers.
                Where(c => bind.ShopBeerIds.Contains(c.Id)).
                ToArrayAsync(cancellationToken);
            for (int i = 0; i < shopBeers.Length; i++)
            {
                shopBeers[i].SourceBeerId = sourceBeerId;
            }
            await context.SaveChangesAsync(cancellationToken);
        }
        public async Task<IEnumerable<ShopBeerInfo>> GetNotBindedShopBeers(SourceBeerDetailsToBind sourceBeerDetails,
            CancellationToken cancellationToken)
        {
            var shopBeers = await GetNotBindedShopBeers(sourceBeerDetails.Name, cancellationToken);
            return shopBeers.Select(b => BeerInfoConverter.ConvertToShopBeerInfo(b));
        }
        public async Task<bool> TryBindShopBeers(SourceBeerDetailsToBind sourceBeerDetails, CancellationToken cancellationToken)
        {
            var shopBeers = await GetNotBindedShopBeers(sourceBeerDetails.Name, cancellationToken);
            foreach (var shopBeer in shopBeers)
            {
                shopBeer.SourceBeerId = sourceBeerDetails.SourceBeerId;
            }
            await context.SaveChangesAsync(cancellationToken);
            return shopBeers.Length > 0;
        }
        private async Task<ShopBeer[]> GetNotBindedShopBeers(string sourceBeerName, CancellationToken cancellationToken)
        {
            var conversion = new Conversion();
            var nameFormater = new BeerNameFormater();
            var beerNameFormated = nameFormater.Format(sourceBeerName).Name;
            var beerNameEn = conversion.RussianCyrillicToLatin(beerNameFormated);
            return await context.ShopBeers.
               Where(c => c.SourceBeerId == null
                     && EF.Functions.ToTsVector("english", c.FormatedName).Matches(beerNameEn)).
               ToArrayAsync(cancellationToken);

        }
    }
}
