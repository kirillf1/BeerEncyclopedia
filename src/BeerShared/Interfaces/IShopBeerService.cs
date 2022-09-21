using BeerShared.Data;
using BeerShared.DTO;
using BeerShared.Queries;

namespace BeerShared.Interfaces
{
    public interface IShopBeerService
    {
        Task<bool> AddBeer(ShopBeerInfo beer);
        Task<bool> AddBeers(IEnumerable<ShopBeerInfo> beerCollection);
        Task DeleteBeer(int id);
        Task<ApiResult<ShopBeerInfo>> GetShopBeers(ShopBeerQuery shopBeerQuery);
        Task UpdateBeer(ShopBeerInfo beer);
    }
}