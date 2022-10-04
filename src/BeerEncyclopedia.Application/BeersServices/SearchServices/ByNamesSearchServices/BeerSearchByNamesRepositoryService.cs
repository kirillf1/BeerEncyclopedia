using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Application.Specifications.Beers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.ByNamesSearchServices
{
    public class BeerSearchByNamesRepositoryService : IBeerSearchByNamesService
    {
        private readonly IRepository<Beer> repository;
        private readonly INameSearchSpecificationFactory<Beer> specificationFactory;

        public BeerSearchByNamesRepositoryService(IRepository<Beer> repository, INameSearchSpecificationFactory<Beer> specificationFactory)
        {
            this.repository = repository;
            this.specificationFactory = specificationFactory;
        }
        public async Task<Result<IEnumerable<BeerLabel>>> SearchBeerLabels(IEnumerable<string> names, int matchCount = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var validateResult = BeerSearchByNamesHelper.Validate(names, matchCount);
                if (!validateResult.IsSuccess)
                    return validateResult;
                var specification = new BeerLabelByNamesSpec(specificationFactory, names, matchCount);
                var beers = await repository.ListAsync(specification, cancellationToken);
                if (!beers.Any())
                    return Result.NotFound();
                return beers;
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
