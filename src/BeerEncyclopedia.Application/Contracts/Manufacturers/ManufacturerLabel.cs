using BeerEncyclopedia.Application.Contracts.Countries;

namespace BeerEncyclopedia.Application.Contracts.Manufacturers
{
    public class ManufacturerLabel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? PictureUrl { get; set; }
        public CountryDto Country { get; set; } = default!;
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if(obj as ManufacturerLabel == null) 
                return false;
            return obj.GetHashCode() == GetHashCode();
        }
    }
}
