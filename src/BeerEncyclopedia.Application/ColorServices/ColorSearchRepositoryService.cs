using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Colors;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Application.Specifications;
using BeerEncyclopedia.Application.Specifications.Colors;
using BeerEncyclopedia.Domain;


namespace BeerEncyclopedia.Application.ColorServices
{
    public class ColorSearchRepositoryService : IColorSearchService
    {
        private readonly IRepository<Color> repository;

        public ColorSearchRepositoryService(IRepository<Color> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<IEnumerable<ColorDto>>> GetAllColors(CancellationToken cancellationToken = default)
        {
            try
            {
                var colors = await repository.ListAsync(cancellationToken);
                if (!colors.Any())
                    return Result.NotFound();
                return Result.Success(colors.Select(c => ColorDtoConverter.ConvertColorToDto(c)));
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<ColorDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var specification = new EntityByIdSpec<Color, ColorDto>(id, c => ColorDtoConverter.ConvertColorToDto(c));
                var color = await repository.FirstOrDefaultAsync(specification,cancellationToken);
                if (color == null)
                    return Result.NotFound();
                return color;
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ColorDto>>> GetByName(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var specification = new ColorByNameSpec<ColorDto>(name, c => ColorDtoConverter.ConvertColorToDto(c));
                var colors = await repository.ListAsync(specification,cancellationToken);
                if (!colors.Any())
                    return Result.NotFound();
                return colors;
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
