using Ardalis.Specification;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Specifications
{
    public interface INameSearchSpecificationFactory<T> where T: Entity
    {
        public Specification<T> GetByNameSpecification(string name);
    }
}
