namespace BeerEncyclopedia.Application.Contracts.Styles
{
    public class StyleLabel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj as StyleLabel == null)
                return false;
            return obj.GetHashCode() == GetHashCode();
        }
    }
}
