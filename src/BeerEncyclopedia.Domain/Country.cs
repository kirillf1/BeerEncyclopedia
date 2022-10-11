namespace BeerEncyclopedia.Domain
{
    public class Country : Entity
    {
        private Country() { }
        public Country(Guid id, string name,string alpha2)
        {
            Id = id;
            Name = name;
            Alpha2 = alpha2;
            Beers = new List<Beer>();
        }
        public string Name { get; set; }
        public string Alpha2 { get; set; }
        public List<Beer> Beers { get; set; }
    }
}
