using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Application.Helpers;

namespace BeerEncyclopedia.Application.CountryServices
{
    public class CountrySearchHttpService : ICountrySearchService
    {
        private readonly HttpClient httpClient;

        public CountrySearchHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<IEnumerable<CountryDto>>> GetAllCountries(CancellationToken cancellationToken = default)
        {
            return await HttpHelper.GetAsObject<IEnumerable<CountryDto>>(httpClient, httpClient.BaseAddress!.ToString(), cancellationToken);
        }

        public async Task<Result<CountryDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await HttpHelper.GetAsObject<CountryDto>(httpClient,
               httpClient.BaseAddress!.ToString() + $"/{id}", cancellationToken);
        }

        public async Task<Result<IEnumerable<CountryDto>>> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await HttpHelper.GetAsObject<IEnumerable<CountryDto>>(httpClient,
                httpClient.BaseAddress!.ToString() + $"?name={name}", cancellationToken);
        }
    }
}
