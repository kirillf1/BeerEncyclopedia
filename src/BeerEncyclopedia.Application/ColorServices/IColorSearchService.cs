using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Colors;

namespace BeerEncyclopedia.Application.ColorServices
{
    public interface IColorSearchService
    {
        public Task<Result<IEnumerable<ColorDto>>> GetAllColors(CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable< ColorDto>>> GetByName(string name, CancellationToken cancellationToken = default);
        public Task<Result<ColorDto>> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}
