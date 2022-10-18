using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications.Styles
{
    public class StylesDetailsByIdSpec : Specification<Style, StyleDetails>, ISingleResultSpecification
    {
        public StylesDetailsByIdSpec(Guid id, int beerCount)
        {
            Query.AsNoTracking().AsSplitQuery();
            Query.Where(x => x.Id == id);
            Query.Include(c => c.Beers)
                .ThenInclude(c => c.Manufacturers)
                .Include(c => c.Beers)
                .ThenInclude(c => c.Country);
            Query.Select(m => StyleDtoConverter.ConvertStyleToDetails(m, beerCount));
        }
    }
}
