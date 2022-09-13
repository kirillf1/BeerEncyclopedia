
namespace BeerEncyclopedia.Domain
{
    public class Beer
    {
        private Beer() { }
        public Beer(Guid id, string name,string pictureUrl, ChemicalIndicators chemicalIndicators, OrganolepticIndicators organolepticIdicators)
        {
            Id = id;
            Name = name;
            ChemicalIndicators = chemicalIndicators;
            OrganolepticIndicators = organolepticIdicators;
            Manufacturers = new List<Manufacturer>();
            BeerImages = new BeerImages(pictureUrl);
            Rating = 0;
            Styles = new List<Style>();
        }
        public Guid Id { get; }
        public BeerImages BeerImages { get; set; }
        public string Name { get; set; }
        public string? AltName { get; set; }
        public Country? Country { get; set; }
        public List<Style> Styles { get; set; } 
        public string? Description { get; set; }
        public double Rating { get; set; }
        public DateOnly? CreationTime { get; set; }
        public ChemicalIndicators ChemicalIndicators { get; set; }
        public OrganolepticIndicators OrganolepticIndicators { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
    }
}
