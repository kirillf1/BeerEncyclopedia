using BeerShared.DTO;

namespace BeerShared.Interfaces
{
    public interface IShopBeerBinder
    {
        Task BindBeers(BindedSourceBeer bind, CancellationToken cancellationToken);
        Task<IEnumerable<ShopBeerInfo>> GetNotBindedShopBeers(SourceBeerDetailsToBind sourceBeerDetails, CancellationToken cancellationToken);
        Task<bool> TryBindShopBeers(SourceBeerDetailsToBind sourceBeerDetails, CancellationToken cancellationToken);
    }
}