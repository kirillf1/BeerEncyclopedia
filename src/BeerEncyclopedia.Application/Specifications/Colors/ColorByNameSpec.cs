using Ardalis.Specification;
using BeerEncyclopedia.Domain;
using System.Linq.Expressions;

namespace BeerEncyclopedia.Application.Specifications.Colors
{
    public class ColorByNameSpec<T> : Specification<Color, T>
    {
        public ColorByNameSpec(string name, Expression<Func<Color, T>> convert)
        {
            Query.AsNoTracking();
            Query.Search(c=>c.Name.ToLower(), $"%{name.ToLower()}%");
            Query.Select(convert);
        }
    }
}
