namespace BeerEncyclopedia.Domain
{
    public class Style
    {
        private Style() { }
        public Style(Guid id,string name, string description)
        {
            Id = id;
            NameEn = name;
            Description = description;
        }
        public Guid Id { get; }
        public string NameEn { get; set; }
        public string? NameRus { get; set; }
        public string Description { get; set; }
    }
}
