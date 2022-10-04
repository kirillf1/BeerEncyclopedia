namespace BeerEncyclopedia.Domain
{
    public class Manufacturer : Entity
    {
        private Manufacturer() { }
        public Manufacturer(Guid id,string name,string description,Country country)
        {
            Id = id;
            Name = name;
            Description = description;
            Country = country;
            Beers = new List<Beer>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public string? PictureUrl { get; set; }
        public List<Beer> Beers { get; set; }
    }
}
