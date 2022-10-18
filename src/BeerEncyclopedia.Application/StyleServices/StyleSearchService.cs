using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Application.Specifications.Styles;
using BeerEncyclopedia.Domain;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.StyleServices
{
    public class StyleSearchService : IStyleSearchService
    {
        private readonly IRepository<Style> repository;

        public StyleSearchService(IRepository<Style> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<ApiResult<StyleLabel>>> SearchStyleLabel(StyleQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var isValid = query.Validate(out var errors);
                if (!isValid)
                    return Result.Invalid(errors);
                var specification = new StylesLabelByQuerySpec(query);
                var count = await repository.CountAsync(specification, cancellationToken);
                if (count == 0)
                    return Result.NotFound();
                var styles = await repository.ListAsync(specification, cancellationToken);
                return new ApiResult<StyleLabel>(styles, count, query.PageIndex, query.PageSize);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        public async Task<Result<StyleDetails>> GetStyleDetails(Guid id,CancellationToken cancellationToken)
        {
            try
            {
                var specification = new StylesDetailsByIdSpec(id, 10);
                var style = await repository.FirstOrDefaultAsync(specification, cancellationToken);
                if (style == null)
                    return Result.NotFound();
                return style;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
