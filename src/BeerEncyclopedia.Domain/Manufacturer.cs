namespace BeerEncyclopedia.Domain
{
    public class Manufacturer
    {
        private Manufacturer() { }
        public Manufacturer(Guid id,string name,string description,Country country)
        {
            Id = id;
            Name = name;
            Description = description;
            Country = country;
        }
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public string? PictureUrl { get; set; }
    }
}
