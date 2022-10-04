using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices
{
    public interface IBeerSearchByIdService
    {
        Task<Result<BeerDetails>> GetBeerDetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}