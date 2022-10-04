using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Styles;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.StyleServices
{
    public interface IStyleSearchService
    {
        Task<Result<StyleDetails>> GetStyleDetails(Guid id,CancellationToken cancellationToken);
        Task<Result<ApiResult<StyleLabel>>> SearchStyleLabel(StyleQuery query, CancellationToken cancellationToken);
    }
}