namespace BeerEncyclopedia.Domain
{
    public class Country
    {
        private Country() { }
        public Country(Guid id, string name)
        {
            Id = id;
            Name = name;
            Beers = new List<Beer>();
        }
        public Guid Id { get; }
        public string Name { get; set; }
        public List<Beer> Beers { get; set; }
    }
}
