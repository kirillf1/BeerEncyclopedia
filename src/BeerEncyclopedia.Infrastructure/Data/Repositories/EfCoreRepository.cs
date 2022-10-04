using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Infrastructure.Data.Repositories
{
    public class EfCoreRepository<T> : RepositoryBase<T>, IReadRepositoryBase<T>, IRepository<T> where T : Entity
    {
        public EfCoreRepository(BeerEncyclopediaDbContext dbContext) : base(dbContext)
        {
        }
    }
}
