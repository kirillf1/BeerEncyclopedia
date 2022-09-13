namespace BeerEncyclopedia.Domain
{
    public class Country
    {
        public Country(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; }
        public string Name { get; set; }
    }
}
