using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices
{
    public interface IBeerSearchByQueryService
    {
        Task<Result<ApiResult<BeerLabel>>> SearchBeerLabels(BeersQuery beersQuery, CancellationToken cancellationToken);
    }
}
