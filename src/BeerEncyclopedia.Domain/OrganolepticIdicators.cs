namespace BeerEncyclopedia.Domain
{
    public class OrganolepticIdicators
    {
        public OrganolepticIdicators(Color color,string taste, double bitterness)
        {
            Color = color;
            Taste = taste;
            Bitterness = bitterness;
        }

        public Color Color { get; set; }
        public string Taste { get; set; }
        public double Bitterness { get; set; }
        public static OrganolepticIdicators DefaultOrganolepticIdicators = new(new Color("Светлое"),"unknown", 0);
    }
}
