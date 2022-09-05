namespace ShopBeerService.Queries
{
    public class ShopBeerQuery
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public bool? IsAvalible { get; set; }
        public bool? HasSourceBeer { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public IEnumerable<Guid>? ShopIds { get; set; }
    }
}
