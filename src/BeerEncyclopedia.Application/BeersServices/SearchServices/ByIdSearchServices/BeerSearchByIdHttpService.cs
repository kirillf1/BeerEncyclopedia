using Ardalis.Result;
using BeerEncyclopedia.Application.BeersServices.SearchServices;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.ByIdSearchServices
{
    public class BeerSearchByIdHttpService : IBeerSearchByIdService
    {
        private readonly HttpClient httpClient;

        public BeerSearchByIdHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<BeerDetails>> GetBeerDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await httpClient.GetAsObject<BeerDetails>(httpClient.BaseAddress!.ToString() + id.ToString(), cancellationToken);
        }
    }
}
