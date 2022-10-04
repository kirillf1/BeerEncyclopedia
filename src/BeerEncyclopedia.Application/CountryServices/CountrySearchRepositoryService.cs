using Ardalis.Result;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Domain;
using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Application.Specifications.Countries;

namespace BeerEncyclopedia.Application.CountryServices
{
    public class CountrySearchRepositoryService : ICountrySearchService
    {
        private readonly IRepository<Country> repository;

        public CountrySearchRepositoryService(IRepository<Country> repository)
        {
            this.repository = repository;
        }


        public async Task<Result<IEnumerable<CountryDto>>> GetAllCountries(CancellationToken cancellationToken = default)
        {
            try
            {
                var colors = await repository.ListAsync(cancellationToken);
                if (!colors.Any())
                    return Result.NotFound();
                return Result.Success(colors.Select(c => CountryDtoConverter.ConvertCountryToDto(c)));
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<CountryDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var specification = new EntityByIdSpec<Country, CountryDto>(id, c => CountryDtoConverter.ConvertCountryToDto(c));
                var country = await repository.FirstOrDefaultAsync(specification, cancellationToken);
                if (country == null)
                    return Result.NotFound();
                return country;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CountryDto>>> GetByName(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var specification = new CountryByNameSpec<CountryDto>(name, c => CountryDtoConverter.ConvertCountryToDto(c));
                var country = await repository.ListAsync(specification, cancellationToken);
                if (!country.Any())
                    return Result.NotFound();
                return country;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
