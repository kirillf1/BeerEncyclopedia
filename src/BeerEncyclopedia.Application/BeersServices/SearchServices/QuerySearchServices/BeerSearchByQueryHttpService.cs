using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.QuerySearchServices
{
    public class BeerSearchByQueryHttpService : IBeerSearchByQueryService
    {
        private readonly HttpClient httpClient;
        public BeerSearchByQueryHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<ApiResult<BeerLabel>>> SearchBeerLabels(BeersQuery beersQuery, CancellationToken cancellationToken)
        {
            var isValid = beersQuery.Validate(out var errors);
            if (!isValid)
                return Result.Invalid(errors);
            var uriQuery = HttpHelper.ConvertQueryToUri(httpClient.BaseAddress!.ToString(), beersQuery);
            return await httpClient.GetAsObject<ApiResult<BeerLabel>>(uriQuery.ToString(), cancellationToken);
        }
    }
}
