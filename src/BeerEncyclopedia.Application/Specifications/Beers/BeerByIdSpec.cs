using Ardalis.Specification;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications.Beers
{
    public class BeerByIdSpec : Specification<Beer>
    {
        public BeerByIdSpec(Guid id)
        {
            Query.Where(c => c.Id == id)
                .Include(c => c.Manufacturers)
                .Include(c => c.ChemicalIndicators)
                .Include(c => c.OrganolepticIndicators)
                .Include(c => c.Country)
                .Include(c => c.Styles)
                .Include(c => c.BeerImages);
        }
    }
}
