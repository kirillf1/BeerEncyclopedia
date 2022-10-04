using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Specifications.Beers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.ByIdSearchServices
{
    public class BeerSearchByIdRepositoryService : IBeerSearchByIdService
    {
        private readonly IRepository<Beer> repository;

        public BeerSearchByIdRepositoryService(IRepository<Beer> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<BeerDetails>> GetBeerDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var beerSpecification = new BeerDetailsByIdSpec(id);
                var beer = await repository.FirstOrDefaultAsync(beerSpecification, cancellationToken);
                if (beer == null)
                    return Result.NotFound();
                return beer;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

    }
}
