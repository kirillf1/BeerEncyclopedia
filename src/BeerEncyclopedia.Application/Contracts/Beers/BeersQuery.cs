using Ardalis.Result;

namespace BeerEncyclopedia.Application.Contracts.Beers
{
    public class BeersQuery : QueryBase
    {
        public string? Name { get; set; }
        public double? RatingMax { get; set; }
        public double? RatingMin { get; set; }
        public List<Guid> StylesId { get; set; } = new List<Guid>();
        public List<Guid> CountriesId { get; set; } = new List<Guid>();
        public List<Guid> ManufacturersId { get; set; } = new List<Guid>();
        public override bool Validate(out List<ValidationError>? errors)
        {
            base.Validate(out errors);
            if (RatingMax.HasValue && RatingMax.Value <= 0)
            {
                errors ??= new List<ValidationError>();
                errors.Add(new ValidationError
                {
                    Identifier = nameof(RatingMax),
                    ErrorMessage = $"{nameof(RatingMax)} must not be less than 0."
                });
            }
            return errors == null;
        }
    }
}
