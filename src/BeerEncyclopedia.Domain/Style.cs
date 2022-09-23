
namespace BeerEncyclopedia.Domain
{
    public class Style : Entity
    {
        private Style() { }
        public Style(Guid id,string name, string description)
        {
            Id = id;
            NameEn = name;
            Description = description;
            Beers = new List<Beer>();
        }
        public List<Beer> Beers { get; set; }
        public string NameEn { get; set; }
        public string? NameRus { get; set; }
        public string Description { get; set; }
    }
}
