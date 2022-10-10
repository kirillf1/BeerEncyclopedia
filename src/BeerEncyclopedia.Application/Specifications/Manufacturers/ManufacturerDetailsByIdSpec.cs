using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications.Manufacturers
{
    public class ManufacturerDetailsByIdSpec: Specification<Manufacturer, ManufacturerDetails>, ISingleResultSpecification
    {
        public ManufacturerDetailsByIdSpec(Guid id,int beerCount)
        {
            Query.AsNoTracking().AsSplitQuery();
            Query.Include(c => c.Country);
            Query.Where(x => x.Id == id);
            Query.Include(c => c.Beers).ThenInclude(c=>c.Styles).Include(c=>c.Beers).ThenInclude(c=>c.Country);
            Query.Select(m=> ManufactureDtoConverter.ConvertManufacturerToDetails(m,beerCount));
        }
    }
}
