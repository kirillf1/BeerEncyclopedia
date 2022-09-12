using Ardalis.Specification;

namespace BeerEncyclopedia.Domain
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class
    {
    }
}
