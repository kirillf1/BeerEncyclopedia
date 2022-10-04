namespace BeerEncyclopedia.Application.Contracts.Countries
{
    public class CountryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
