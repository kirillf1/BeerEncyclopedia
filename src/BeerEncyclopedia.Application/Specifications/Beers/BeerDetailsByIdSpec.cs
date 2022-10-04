using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications.Beers
{
    public class BeerDetailsByIdSpec : Specification<Beer, BeerDetails>, ISingleResultSpecification
    {
        public BeerDetailsByIdSpec(Guid id)
        {
            Query.AsNoTracking();
            Query.Where(c => c.Id == id)
                .Include(c => c.Manufacturers)
                .Include(c => c.ChemicalIndicators)
                .Include(c => c.OrganolepticIndicators)
                .Include(c => c.Country)
                .Include(c => c.Styles)
                .Include(c => c.BeerImages);
            Query.Select(b => BeerDtoConventer.ConvertBeerToBeerDetails(b));
        }
    }
}
