using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications.Manufacturers
{
    public class ManufacturersLabelByQuerySpec : Specification<Manufacturer, ManufacturerLabel>
    {
        public ManufacturersLabelByQuerySpec(ManufacturerQuery manufacturerQuery,
            INameSearchSpecificationFactory<Manufacturer> nameSpecFactory)
        {
            Query.AsNoTracking();
            if (manufacturerQuery.Name != null)
            {
                var specificationByName = nameSpecFactory.GetByNameSpecification(manufacturerQuery.Name);
                foreach (var filter in specificationByName.WhereExpressions.Select(c => c.Filter))
                    Query.Where(filter);
            }
            if (manufacturerQuery.CountriesId != null)
            {
                Query.Where(m => manufacturerQuery.CountriesId.Contains(m.Country.Id));
            }
            Query.Select(m => ManufactureDtoConverter.ConvertManufacturerToLabel(m));
            Query.SkipTake(manufacturerQuery);
            Query.OrderBy(c => c.Name);
        }
    }
}
