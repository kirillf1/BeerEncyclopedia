using Ardalis.Result;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.ByNamesSearchServices
{
    internal static class BeerSearchByNamesHelper
    {
        public static Result Validate(IEnumerable<string> names, int matchCount )
        {
            if (!names.Any())
            {
                return Result.Invalid(new List<ValidationError> { new ValidationError
                    {
                        Identifier = nameof(names),
                        ErrorMessage = $"{nameof(names)} is empty!"
                    } });
            }
            if(matchCount < 1)
            {
                return Result.Invalid(new List<ValidationError> { new ValidationError
                    {
                        Identifier = nameof(names),
                        ErrorMessage = $"{nameof(names)} is empty!"
                    } });
            }
            return Result.Success();
        }
    }
}
