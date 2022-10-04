using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices
{
    public interface IBeerSearchByNamesService
    {
        public Task<Result<IEnumerable<BeerLabel>>> SearchBeerLabels(IEnumerable<string> names, int matchCount = 10, CancellationToken cancellationToken = default);
    }
}
