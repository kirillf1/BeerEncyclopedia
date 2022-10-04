using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Countries;

namespace BeerEncyclopedia.Application.CountryServices
{
    public interface ICountrySearchService
    {
        public Task<Result<IEnumerable<CountryDto>>> GetAllCountries(CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<CountryDto>>> GetByName(string name, CancellationToken cancellationToken = default);
        public Task<Result<CountryDto>> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}
