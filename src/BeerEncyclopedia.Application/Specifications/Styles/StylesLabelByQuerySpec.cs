using Ardalis.Specification;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Application.Helpers;
using BeerEncyclopedia.Domain;
using System.Globalization;

namespace BeerEncyclopedia.Application.Specifications.Styles
{
    public class StylesLabelByQuerySpec : Specification<Style, StyleLabel>
    {
        public StylesLabelByQuerySpec(StyleQuery styleQuery)
        {
            Query.AsNoTracking();
            if (styleQuery.Name != null)
            {
                Query.Search(c => c.NameEn + c.NameRus, $"%{styleQuery.Name}%");
            }
            Query.Select(s => StyleDtoConverter.ConvertStyleToLabel(s));
            Query.SkipTake(styleQuery);
            Query.OrderByDescending(c => c.NameRus != null).ThenBy(c => c.NameRus).ThenBy(c => c.NameEn);
        }
    }
}
