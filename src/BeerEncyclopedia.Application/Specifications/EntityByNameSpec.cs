using Ardalis.Specification;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications
{
    public class EntityByNameSpec<T> : Specification<T> where T : Entity
    {
        public EntityByNameSpec(string name, Action<string, ISpecificationBuilder<T>> queryAction)
        {
            queryAction(name, Query);
        }
    }
}
