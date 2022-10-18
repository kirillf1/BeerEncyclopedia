using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Helpers
{
    public class StyleDtoConverter
    {
        public static StyleDetails ConvertStyleToDetails(Style style, int beerCount)
        {
            return new StyleDetails
            {
                Id = style.Id,
                NameEn = style.NameEn,
                NameRus = style.NameRus,
                Description = style.Description,
                Beers = style.Beers.Select(b => BeerDtoConventer.ConvertBeerToLabel(b)).Take(beerCount)
            };
        }
        public static StyleLabel ConvertStyleToLabel(Style style)
        {
            return new StyleLabel
            {
                Id = style.Id,
                NameEn =  style.NameEn,
                NameRus = style.NameRus
            };
        }
    }
}
