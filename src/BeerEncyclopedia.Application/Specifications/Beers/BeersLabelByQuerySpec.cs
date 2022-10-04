using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications.Beers
{
    public class BeersLabelByQuerySpec : Specification<Beer, BeerLabel>
    {
        static readonly Dictionary<string, Func<OrderBy, ISpecificationBuilder<Beer, BeerLabel>,
            IOrderedSpecificationBuilder<Beer>>> OrderColumn = new(StringComparer.InvariantCultureIgnoreCase);
        static BeersLabelByQuerySpec()
        {
            OrderColumn["Name"] = (o, b) => b.SetOrderExpression(o, c => c.Name);
            OrderColumn["Rating"] = (o, b) => b.SetOrderExpression(o, c => c.Rating);
            OrderColumn["CreationTime"] = (o, b) => b.SetOrderExpression(o, c => c.CreationTime);
        }
        public BeersLabelByQuerySpec(BeersQuery beersQuery, INameSearchSpecificationFactory<Beer> specificationFactory)
        {
            Query.AsNoTracking();
            Query.Include(c => c.Styles)
                .Include(c => c.Manufacturers).
                Include(c => c.Country);
            if (beersQuery.Name != null)
            {
                var specificationByName = specificationFactory.GetByNameSpecification(beersQuery.Name);
                foreach (var filter in specificationByName.WhereExpressions.Select(c => c.Filter))
                    Query.Where(filter);
            }
            if (beersQuery.RatingMax.HasValue)
                Query.Where(c => c.Rating <= beersQuery.RatingMax);
            if (beersQuery.RatingMin.HasValue)
                Query.Where(c => c.Rating >= beersQuery.RatingMin);
            if (beersQuery.StylesId != null && beersQuery.StylesId.Any())
                Query.Where(s => s.Styles.Any(c => beersQuery.StylesId.Contains(c.Id)));
            if (beersQuery.ManufacturersId != null && beersQuery.ManufacturersId.Any())
                Query.Where(s => s.Manufacturers.Any(c => beersQuery.ManufacturersId.Contains(c.Id)));
            if (beersQuery.CountriesId != null && beersQuery.CountriesId.Any())
                Query.Where(s => beersQuery.CountriesId.Contains(s.Country.Id));
            var orderString = beersQuery.OrderColumn ?? "Name";
            if (OrderColumn.ContainsKey(orderString))
            {
                var orderMethod = OrderColumn[orderString];
                orderMethod(beersQuery.Order, Query).ThenByDescending(c => c.CreationTime.HasValue);
            }
            Query.Select(b => BeerDtoConventer.ConvertBeerToLabel(b));
            Query.SkipTake(beersQuery);
        }

    }
}
