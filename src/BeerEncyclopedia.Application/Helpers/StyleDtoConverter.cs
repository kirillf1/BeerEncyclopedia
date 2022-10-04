using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Helpers
{
    public class StyleDtoConverter
    {
        public static StyleDetails ConvertStyleToDetails(Style style)
        {
            return new StyleDetails
            {
                Id = style.Id,
                NameEn = style.NameEn,
                NameRus = style.NameRus,
                Description = style.Description
            };
        }
        public static StyleLabel ConvertStyleToLabel(Style style)
        {
            return new StyleLabel
            {
                Id = style.Id,
                Name = style.NameRus ?? style.NameEn,
            };
        }
    }
}
