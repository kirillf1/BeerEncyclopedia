namespace BeerEncyclopedia.Domain
{
    public class Country : Entity
    {
        private Country() { }
        public Country(Guid id, string name)
        {
            Id = id;
            Name = name;
            Beers = new List<Beer>();
        }
        public string Name { get; set; }
        public List<Beer> Beers { get; set; }
    }
}
