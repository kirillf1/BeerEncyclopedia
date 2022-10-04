using BeerEncyclopedia.Application.Contracts.Colors;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Helpers
{
    public static class ColorDtoConverter
    {
        public static ColorDto ConvertColorToDto(Color color)
        {
            return new ColorDto
            {
                Id = color.Id,
                Name = color.Name
            };
        }
    }
}
