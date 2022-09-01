namespace BeerEncyclopedia.Domain
{
    public class Brand
    {
        public Brand(Guid id,string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
