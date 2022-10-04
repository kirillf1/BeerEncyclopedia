using BeerEncyclopedia.Application.Contracts.Colors;
using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Contracts.Styles;

namespace BeerEncyclopedia.Application.Contracts.Beers
{
    public class BeerDetails
    {
        public string Name { get; set; } = default!;
        public string? AltName { get; set; }
        public string? Description { get; set; }
        public CountryDto CountryDto { get; set; } = default!;
        public ColorDto ColorDto { get; set; } = default!;
        public double Rating { get; set; }
        public string? CreationTime { get; set; }
        public List<StyleLabel> Styles { get; set; } = default!;
        public List<ManufacturerLabel> Manufacturers { get; set; } = default!;
        public string MainImageUrl { get; set; } = default!;
        public List<string> AdditionalImages { get; set; } = default!;
        public bool Filtration { get; set; }
        public bool Pasteurization { get; set; }
        public double Strength { get; set; }
        public double InitialWort { get; set; }
        public string? Taste { get; set; }
        public double Bitterness { get; set; }

    }
}
