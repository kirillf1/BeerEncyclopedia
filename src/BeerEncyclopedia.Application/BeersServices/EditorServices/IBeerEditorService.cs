using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;

namespace BeerEncyclopedia.Application.BeersServices.EditorServices
{
    public interface IBeerEditorService
    {
        Task<Result> AddBeer(BeerDetails beerDetails, CancellationToken cancellationToken = default);
        Task<Result> UpdateBeer(BeerDetails beerDetails, CancellationToken cancellationToken = default);
        Task<Result> RemoveBeer(Guid id, CancellationToken cancellationToken = default);
    }
}
