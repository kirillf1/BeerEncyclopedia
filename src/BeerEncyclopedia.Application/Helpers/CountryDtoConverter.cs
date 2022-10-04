using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Helpers
{
    public static class CountryDtoConverter
    {
        public static CountryDto ConvertCountryToDto(Country country)
        {
            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name
            };
        }
    }
}
