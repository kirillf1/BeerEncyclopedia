using Ardalis.Specification;
using BeerEncyclopedia.Domain;
using System.Linq.Expressions;

namespace BeerEncyclopedia.Application.Specifications
{
    public class EntityByIdSpec<T, V> : Specification<T, V>, ISingleResultSpecification where T : Entity
    {
        public EntityByIdSpec(Guid id, Expression<Func<T, V>> convert)
        {
            Query.Where(x => x.Id == id);
            Query.Select(convert);
        }
    }
}
