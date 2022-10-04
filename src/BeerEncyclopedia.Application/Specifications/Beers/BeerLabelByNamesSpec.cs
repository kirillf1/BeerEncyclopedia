using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;
using System.Linq.Expressions;

namespace BeerEncyclopedia.Application.Specifications.Beers
{
    public class BeerLabelByNamesSpec : Specification<Beer, BeerLabel>
    {
        public BeerLabelByNamesSpec(INameSearchSpecificationFactory<Beer> nameSpecificationFactory, IEnumerable<string> names, int count = 10)
        {
            Query.AsNoTracking();
            Query.Include(c => c.Styles)
               .Include(c => c.Manufacturers)
               .Include(c => c.Country);
            var expressions = names
                .Select(n => nameSpecificationFactory.GetByNameSpecification(n))
                .SelectMany(c => c.WhereExpressions)
                .Select(c => c.Filter).ToList();
            var joinedExpression = SpecificationBuilderExtensions.JoinQueries(Expression.Or, expressions);
            Query.Where(joinedExpression);
            Query.Take(count);
            Query.Select(b => BeerDtoConventer.ConvertBeerToLabel(b));
        }

    }
}
