namespace BeerFormaters.BeerNameFormater
{
    public sealed class FormatNameResult
    {
        public FormatNameResult(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public double? Volume { get; set; }
        public string? Color { get; set; }
        public string? Country { get; set; }
        public bool? Filtration { get; set; }
        public string? Style { get; set; }
        public string? Manufacturer { get; set; }
        public bool? Pasteurization { get; set; }
        public double? Strenght { get; set; }
    }
}
