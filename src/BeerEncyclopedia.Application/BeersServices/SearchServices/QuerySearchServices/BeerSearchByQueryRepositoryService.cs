using Ardalis.Result;
using BeerEncyclopedia.Application.BeersServices.SearchServices;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Application.Specifications.Beers;
using BeerEncyclopedia.Domain;
using BeerShared.Data;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.QuerySearchServices
{
    public class BeerSearchByQueryRepositoryService : IBeerSearchByQueryService
    {
        private readonly IRepository<Beer> repository;
        private readonly INameSearchSpecificationFactory<Beer> specificationFactory;

        public BeerSearchByQueryRepositoryService(IRepository<Beer> repository, INameSearchSpecificationFactory<Beer> specificationFactory)
        {
            this.repository = repository;
            this.specificationFactory = specificationFactory;
        }
        public async Task<Result<ApiResult<BeerLabel>>> SearchBeerLabels(BeersQuery beersQuery, CancellationToken cancellationToken)
        {
            try
            {
                var isValid = beersQuery.Validate(out var errors);
                if (!isValid)
                    return Result.Invalid(errors);
                var specification = new BeersLabelByQuerySpec(beersQuery, specificationFactory);
                var count = await repository.CountAsync(specification, cancellationToken);
                if (count == 0)
                    return Result.NotFound();
                var beers = await repository.ListAsync(specification, cancellationToken);
                return new ApiResult<BeerLabel>(beers, count, beersQuery.PageIndex, beersQuery.PageSize);
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }
    }
}
