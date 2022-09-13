namespace BeerEncyclopedia.Domain
{
    public class Color
    {
        public Color(Guid id,string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; set; }
    }
}
