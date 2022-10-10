using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Application.Specifications.Manufacturers;
using BeerEncyclopedia.Domain;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.ManufacturerServices
{
    public class ManufacturerSearchService : IManufacturerSearchService
    {
        private readonly IRepository<Manufacturer> repository;
        private readonly INameSearchSpecificationFactory<Manufacturer> nameSearchSpecificationFactory;

        public ManufacturerSearchService(IRepository<Manufacturer> repository, INameSearchSpecificationFactory<Manufacturer> nameSearchSpecificationFactory)
        {
            this.repository = repository;
            this.nameSearchSpecificationFactory = nameSearchSpecificationFactory;
        }
        public async Task<Result<ApiResult<ManufacturerLabel>>> SearchManufacturerLabel(ManufacturerQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var isValid = query.Validate(out var errors);
                if (!isValid)
                    return Result.Invalid(errors);
                var specification = new ManufacturersLabelByQuerySpec(query, nameSearchSpecificationFactory);
                var count = await repository.CountAsync(specification, cancellationToken);
                if (count == 0)
                    return Result.NotFound();
                var manufacturers = await repository.ListAsync(specification, cancellationToken);
                return new ApiResult<ManufacturerLabel>(manufacturers, count, query.PageIndex, query.PageSize);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        public async Task<Result<ManufacturerDetails>> GetManufacturerDetails(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var specification = new ManufacturerDetailsByIdSpec(id, 10);
                var manufacturer = await repository.FirstOrDefaultAsync(specification, cancellationToken);
                if (manufacturer is null)
                    return Result.NotFound();
                return manufacturer;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
