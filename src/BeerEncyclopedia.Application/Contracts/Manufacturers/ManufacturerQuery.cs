namespace BeerEncyclopedia.Application.Contracts.Manufacturers
{
    public class ManufacturerQuery : QueryBase
    {
        public List<Guid> CountriesId { get; set; } = new List<Guid>();
        public string? Name { get; set; }
    }
}
