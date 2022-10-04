using Ardalis.Result;

namespace BeerEncyclopedia.Application.ImageSearchServices
{
    public interface IImageSearchSevice
    {
        public Task<Result<IEnumerable<string>>> GetNamesByImage(byte[] imageBytes);
    }
}
