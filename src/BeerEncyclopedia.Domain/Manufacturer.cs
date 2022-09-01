namespace BeerEncyclopedia.Domain
{
    public class Manufacturer
    {
        public Manufacturer(Guid id,string name,string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
