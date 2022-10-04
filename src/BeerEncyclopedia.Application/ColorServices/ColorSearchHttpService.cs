using Ardalis.Result;
using BeerEncyclopedia.Application.Contracts.Colors;
using BeerEncyclopedia.Application.Helpers;

namespace BeerEncyclopedia.Application.ColorServices
{
    public class ColorSearchHttpService : IColorSearchService
    {
        private readonly HttpClient httpClient;

        public ColorSearchHttpService(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException(nameof(httpClient.BaseAddress));
            this.httpClient = httpClient;
        }
        public async Task<Result<IEnumerable<ColorDto>>> GetAllColors(CancellationToken cancellationToken = default)
        {
            return await HttpHelper.GetAsObject<IEnumerable<ColorDto>>(httpClient, httpClient.BaseAddress!.ToString(),cancellationToken);
        }

        public async Task<Result<ColorDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await HttpHelper.GetAsObject<ColorDto>(httpClient, 
                httpClient.BaseAddress!.ToString() +$"/{id}" , cancellationToken);
        }

        public async Task<Result<IEnumerable<ColorDto>>> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await HttpHelper.GetAsObject<IEnumerable<ColorDto>>(httpClient,
                httpClient.BaseAddress!.ToString() + $"?name={name}", cancellationToken);
        }
    }
}
