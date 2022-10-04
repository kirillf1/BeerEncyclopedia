namespace BeerEncyclopedia.Application.Contracts.Manufacturers
{
    public class ManufacturerQuery : QueryBase
    {
        public List<Guid>? CountriesId { get; set; } 
        public string? Name { get; set; }
    }
}
