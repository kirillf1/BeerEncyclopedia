using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.ManufacturerServices
{
    public interface IManufacturerSearchService
    {
        Task<Result<ManufacturerDetails>> GetManufacturerDetails(Guid id, CancellationToken cancellationToken);
        Task<Result<ApiResult<ManufacturerLabel>>> SearchManufacturerLabel(ManufacturerQuery query, CancellationToken cancellationToken);
    }
}