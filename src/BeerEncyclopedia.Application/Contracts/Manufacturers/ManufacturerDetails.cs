using BeerEncyclopedia.Application.Contracts.Beers;

namespace BeerEncyclopedia.Application.Contracts.Manufacturers
{
    public class ManufacturerDetails : ManufacturerLabel
    {
        public string? Description { get; set; }
        public IEnumerable<BeerLabel> Beers { get; set; } =  Enumerable.Empty<BeerLabel>();
    }
}
