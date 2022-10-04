using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.ImageSearchServices;
using BeerFormaters.BeerNameFormater;
using Cyrillic.Convert;

namespace BeerEncyclopedia.Application.BeersServices.SearchServices.ByImageSearchServices
{
    public class BeerSearchByImageService : IBeerSearchByImageService
    {
        private readonly IImageSearchSevice imageSearchSevice;
        private readonly IBeerSearchByNamesService beerSearchByNamesService;

        public BeerSearchByImageService(IImageSearchSevice imageSearchSevice, IBeerSearchByNamesService beerSearchByNamesService)
        {
            this.imageSearchSevice = imageSearchSevice;
            this.beerSearchByNamesService = beerSearchByNamesService;
        }
        public async Task<Result<IEnumerable<BeerLabel>>> GetBeersByImage(byte[] imageBytes,
            int count = 10, CancellationToken cancellationToken =default)
        {
            var imageSearchResult = await imageSearchSevice.GetNamesByImage(imageBytes);
            if (!imageSearchResult.IsSuccess)
            {
                return GetInvalidStatus(imageSearchResult);
            }
            var names = imageSearchResult.Value.ToList();
            var formater = new BeerNameFormater();
            var currentNamesCount = names.Count;
            var conversion = new Conversion();
            for (int i = 0; i < currentNamesCount; i++)
            {
                names[i] = formater.Format(names[i]).Name;
                if (!string.IsNullOrEmpty(names[i]))
                    names.Add(conversion.RussianCyrillicToLatin(names[i]));
            }
            return await beerSearchByNamesService.SearchBeerLabels(names.Distinct()
                .Where(s => !string.IsNullOrEmpty(s)), count, cancellationToken);
        }
        private static Result<IEnumerable<BeerLabel>> GetInvalidStatus(Result<IEnumerable<string>> result)
        {
            var resultStatus = result.Status;
            return resultStatus switch
            {
                ResultStatus.Error => (Result<IEnumerable<BeerLabel>>)Result.Error(result.Errors.ToArray()),
                ResultStatus.Forbidden => (Result<IEnumerable<BeerLabel>>)Result.Forbidden(),
                ResultStatus.Unauthorized => (Result<IEnumerable<BeerLabel>>)Result.Unauthorized(),
                ResultStatus.Invalid => (Result<IEnumerable<BeerLabel>>)Result.Invalid(result.ValidationErrors),
                ResultStatus.NotFound => (Result<IEnumerable<BeerLabel>>)Result.NotFound(),
                _ => throw new ArgumentException(nameof(resultStatus)),
            };
        }
    }
}
