using Ardalis.Specification;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeerEncyclopedia.Infrastructure.Data.Specifications
{
    public class BeerFullTextSearchSpecPgFactory : INameSearchSpecificationFactory<Beer>
    {
        public Specification<Beer> GetByNameSpecification(string name)
        {
            return new EntityByNameSpec<Beer>(name, (t, c) =>
            c.Where(p => EF.Functions.ToTsVector("russian", p.Name + " " + p.AltName)
               .Matches(t)));
        }
    }
}
