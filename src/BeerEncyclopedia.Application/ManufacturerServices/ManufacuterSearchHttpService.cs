using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Helpers;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.ManufacturerServices
{
    public class ManufacuterSearchHttpService : IManufacturerSearchService
    {
        private readonly HttpClient httpClient;

        public ManufacuterSearchHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress is null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<ManufacturerDetails>> GetManufacturerDetails(Guid id, CancellationToken cancellationToken)
        {
            return await httpClient.GetAsObject<ManufacturerDetails>(httpClient.BaseAddress!.ToString() + id.ToString(), cancellationToken);
        }

        public async Task<Result<ApiResult<ManufacturerLabel>>> SearchManufacturerLabel(ManufacturerQuery query, CancellationToken cancellationToken)
        {
            var isValid = query.Validate(out var errors);
            if (!isValid)
                return Result.Invalid(errors);
            var uriQuery = HttpHelper.ConvertQueryToUri(httpClient.BaseAddress!.ToString(), query);
            return await httpClient.GetAsObject<ApiResult<ManufacturerLabel>>(uriQuery.ToString(), cancellationToken);
        }
    }
}
