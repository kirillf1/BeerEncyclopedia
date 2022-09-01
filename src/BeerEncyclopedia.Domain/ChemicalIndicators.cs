namespace BeerEncyclopedia.Domain
{
    public class ChemicalIndicators
    {
        public static ChemicalIndicators DefaultChemicalIndicators = new(false,false,0,0);
        public ChemicalIndicators(bool filtration, bool pasteurization, double strength, double initialWort)
        {
            Filtration = filtration;
            Pasteurization = pasteurization;
            Strength = strength;
            InitialWort = initialWort;
        }

        public bool Filtration { get; set; }
        public bool Pasteurization { get; set; }
        public double Strength { get; set; }
        public double InitialWort { get; set; }
    }
}
