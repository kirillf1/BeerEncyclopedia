using Ardalis.Specification;
using BeerEncyclopedia.Domain;
using System.Linq.Expressions;

namespace BeerEncyclopedia.Application.Specifications.Countries
{
    public class CountryByNameSpec<T> : Specification<Country, T>
    {
        public CountryByNameSpec(string name, Expression<Func<Country, T>> convert)
        {
            Query.AsNoTracking();
            Query.Search(c => c.Name.ToLower(), $"%{name.ToLower()}%");
            Query.Select(convert);
        }
    }
}
