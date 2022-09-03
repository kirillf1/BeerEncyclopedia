namespace ShopParsers
{
    public class Shop
    {
        public Shop(Guid id, string name)
        {
            Id = id;
            Name = name;
            ShopBeers = new();
        }
        public Shop(Guid id, string name,IEnumerable<ShopBeer> shopBeers) : this(id,name)
        {
            ShopBeers.AddRange(shopBeers);
        }
        public List<ShopBeer> ShopBeers { get; }
        public Guid Id { get; }
        public string Name { get; set; }
    }
}
