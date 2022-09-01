
namespace BeerEncyclopedia.Domain
{
    public class Beer
    {
        public Beer(Guid id, string name,string url, ChemicalIndicators chemicalIndicators, OrganolepticIdicators organolepticIdicators)
        {
            Id = id;
            Name = name;
            ChemicalIndicators = chemicalIndicators;
            OrganolepticIdicators = organolepticIdicators;
            Manufacturers = new List<Manufacturer>();
            BeerImages = new BeerImages(url);
            Rating = 0;
            Styles = new List<Style>();
        }

        public Guid Id { get; }
        public BeerImages BeerImages { get; set; }
        public string Name { get; set; }
        public string? NameRus { get; set; }
        public Country? Country { get; set; }
        public Brand? Brand { get; set; }
        public List<Style> Styles { get; set; } 
        public string? Description { get; set; }
        public double Rating { get; set; }
        public DateTime? CreationTime { get; set; }
        public ChemicalIndicators ChemicalIndicators { get; set; }
        public OrganolepticIdicators OrganolepticIdicators { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
    }
}
