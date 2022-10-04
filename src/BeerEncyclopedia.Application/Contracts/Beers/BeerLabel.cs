using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.Contracts.Styles;

namespace BeerEncyclopedia.Application.Contracts.Beers
{
    public class BeerLabel
    {
        public List<ManufacturerLabel> Manufacturers { get; set; } = default!;
        public List<StyleLabel> Styles { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public string Name { get; set; } = default!;
        public double Rating { get; set; }
        public string? Country { get; set; } = default!;
        public string? CreationTime { get; set; }
    }
}
