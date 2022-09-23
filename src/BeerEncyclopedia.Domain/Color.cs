namespace BeerEncyclopedia.Domain
{
    public class Color : Entity
    {
        private Color() { }
        public Color(Guid id,string name)
        {
            Id = id;
            Name = name;
            Beers = new List<Beer>();
        }
        public string Name { get; set; }
        public List<Beer> Beers { get; set; }
    }
}
