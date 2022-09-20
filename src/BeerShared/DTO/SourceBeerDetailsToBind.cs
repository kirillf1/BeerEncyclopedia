namespace BeerShared.DTO
{
    public class SourceBeerDetailsToBind
    {
        public SourceBeerDetailsToBind(Guid sourceBeerId,string name, string? manufacturerName)
        {
            SourceBeerId = sourceBeerId;
            Name = name;
            ManufacturerName = manufacturerName;
        }
        public Guid SourceBeerId { get; set; }
        public string Name { get; set; }
        public string? ManufacturerName { get; set; }
    }
}
