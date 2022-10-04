using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices
{
    public interface IBeerSearchByImageService
    {
        public Task<Result<IEnumerable<BeerLabel>>> GetBeersByImage(byte[] imageBytes,int count = 10,CancellationToken cancellationToken = default);
    }
}
