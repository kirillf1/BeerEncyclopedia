using Ardalis.Result;

namespace BeerEncyclopedia.Application.Contracts
{
    public class QueryBase
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 50;
        public OrderBy Order { get; set; } = OrderBy.ASC;
        public string? OrderColumn { get; set; }

        public virtual bool Validate(out List<ValidationError>? errors)
        {
            errors = null;
            if (PageIndex < 0)
            {
                errors ??= new List<ValidationError>();
                errors.Add(new ValidationError
                {
                    Identifier = nameof(PageIndex),
                    ErrorMessage = $"{nameof(PageIndex)} must not be less than 0."
                });
            }
            if (this.PageSize <= 0)
            {
                errors ??= new List<ValidationError>();
                errors.Add(new ValidationError
                {
                    Identifier = nameof(this.PageSize),
                    ErrorMessage = $"{nameof(this.PageSize)} must be more than 0."
                });
            }
            return errors == null;
        }
    }
    public enum OrderBy
    {
        ASC,
        DESC
    }
}
