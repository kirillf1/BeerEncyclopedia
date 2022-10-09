using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Helpers
{
    public static class BeerDtoConventer
    {
        public static BeerLabel ConvertBeerToLabel(Beer beer)
        {
            return new BeerLabel
            {
                Id  = beer.Id,
                Name = beer.Name,
                Country = beer.Country.Name,
                Manufacturers = beer.Manufacturers.Select(c => ManufactureDtoConverter.ConvertManufacturerToLabel(c)).ToList(),
                Rating = beer.Rating,
                PictureUrl = beer.BeerImages.MainImageUrl,
                Styles = beer.Styles.Select(c=> StyleDtoConverter.ConvertStyleToLabel(c)).ToList(),
                CreationTime = beer.CreationTime.ToString()
            };
        }
        public static BeerDetails ConvertBeerToBeerDetails(Beer beer)
        {
            return new BeerDetails
            {
                Id = beer.Id,
                AltName = beer.AltName,
                Name = beer.Name,
                Description = beer.Description,
                Bitterness = beer.OrganolepticIndicators.Bitterness,
                Color = ColorDtoConverter.ConvertColorToDto(beer.OrganolepticIndicators.Color),
                Filtration = beer.ChemicalIndicators.Filtration,
                Pasteurization = beer.ChemicalIndicators.Pasteurization,
                AdditionalImages = beer.BeerImages.ImageUrls,
                MainImageUrl = beer.BeerImages.MainImageUrl,
                Country = CountryDtoConverter.ConvertCountryToDto(beer.Country),
                CreationTime = beer.CreationTime.ToString(),
                InitialWort = beer.ChemicalIndicators.InitialWort,
                Manufacturers = beer.Manufacturers.Select(c => ManufactureDtoConverter.ConvertManufacturerToLabel(c)).ToList(),
                Rating = beer.Rating,
                Strength = beer.ChemicalIndicators.Strength,
                Styles = beer.Styles.Select(s => StyleDtoConverter.ConvertStyleToLabel(s)).ToList(),
                Taste = beer.OrganolepticIndicators.Taste
            };
        }
    }
}
