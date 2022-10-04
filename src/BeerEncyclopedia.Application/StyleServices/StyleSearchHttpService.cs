using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Application.Helpers;
using BeerShared.Data;
using System.Threading;

namespace BeerEncyclopedia.Application.StyleServices
{
    public class StyleSearchHttpService : IStyleSearchService
    {
        private readonly HttpClient httpClient;

        public StyleSearchHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress is null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<StyleDetails>> GetStyleDetails(Guid id, CancellationToken cancellationToken)
        {
            return await httpClient.GetAsObject<StyleDetails>(httpClient.BaseAddress!.ToString() + id.ToString(), cancellationToken);
        }

        public async Task<Result<ApiResult<StyleLabel>>> SearchStyleLabel(StyleQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var isValid = query.Validate(out var errors);
                if (!isValid)
                    return Result.Invalid(errors);
                var uriQuery = HttpHelper.ConvertQueryToUri(httpClient.BaseAddress!.ToString(), query);
                return await httpClient.GetAsObject<ApiResult<StyleLabel>>(uriQuery.ToString(), cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
