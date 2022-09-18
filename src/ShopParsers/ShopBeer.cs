namespace ShopParsers
{
    public class ShopBeer
    {
        public ShopBeer(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public int Id { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public string FormatedName { get; set; } = "";
        public string? Color { get; set; }
        public string? Manufacturer { get; set; }
        public double? Volume { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? Country { get; set; }
        public double? Strength { get; set; }
        public bool? Filtration { get; set; }
        public bool? Pasteurization { get; set; }
        public string? DetailsUrl { get; set; }
        public double? Rating { get; set; }
        public bool IsAvailable { get; set; }
        public string? Style { get; set; }
        public double? InitialWort { get; set; }
        public string? Description { get; set; }
        public Guid? SourceBeerId { get; set; }
       
    }
}
