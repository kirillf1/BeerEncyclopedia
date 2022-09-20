using BeerFormaters.BeerNameFormater;
using BeerShared.DTO;
using Cyrillic.Convert;
using Microsoft.EntityFrameworkCore;
using ShopBeerService.Infrastructure;

namespace ShopBeerService.Services.SourceBeerServices
{
    public class SourceBeerBinder
    {
        public SourceBeerBinder(ShopBeerPGDbContext context)
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
        public async Task<bool> TryBindShopBeers(SourceBeerDetailsToBind sourceBeerDetails, CancellationToken cancellationToken)
        {
            var conversion = new Conversion();
            var nameFormater = new BeerNameFormater();
            var beerNameFormated = nameFormater.Format(sourceBeerDetails.Name).Name;
            var beerNameEn = conversion.RussianCyrillicToLatin(beerNameFormated);
            var shopBeers = await context.ShopBeers.
                Where(c => c.SourceBeerId == null
                      && EF.Functions.ToTsVector("english", c.FormatedName).Matches(beerNameEn)).
                ToArrayAsync(cancellationToken);
            foreach (var shopBeer in shopBeers)
            {
                shopBeer.SourceBeerId = sourceBeerDetails.SourceBeerId;
            }
            await context.SaveChangesAsync(cancellationToken);
            return shopBeers.Length > 0;
        }
    }
}
