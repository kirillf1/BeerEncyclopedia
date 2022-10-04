namespace BeerEncyclopedia.Application.Contracts.Styles
{
    public class StyleDetails
    {
        public Guid Id { get; set; }
        public string NameEn { get; set; } = default!;
        public string? NameRus { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
