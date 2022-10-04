using Ardalis.Specification;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeerEncyclopedia.Infrastructure.Data.Specifications
{
    public class ManufacturerLikeSearchPgFactory : INameSearchSpecificationFactory<Manufacturer>
    {
        public Specification<Manufacturer> GetByNameSpecification(string name)
        {
            return new EntityByNameSpec<Manufacturer>(name, (t, c) =>
           c.Where(p => EF.Functions.ILike(p.Name,$"%{t}%")));
        }
    }
}
