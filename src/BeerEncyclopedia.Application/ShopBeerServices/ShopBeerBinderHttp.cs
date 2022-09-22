using BeerEncyclopedia.Application.Helpers;
using BeerShared.DTO;
using BeerShared.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace BeerEncyclopedia.Application.ShopBeerServices
{
    public class ShopBeerBinderHttp : IShopBeerBinder
    {
        private readonly HttpClient httpClient;
        public ShopBeerBinderHttp(HttpClient httpClient)
        {
            if(httpClient.BaseAddress == null)
                throw new ArgumentNullException("BaseAddress is null");
            this.httpClient = httpClient;
        }
        public async Task BindBeers(BindedSourceBeer bind, CancellationToken cancellationToken)
        {
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress + "/bind", bind, cancellationToken);
            HttpHelper.CheckStatusCode(response.StatusCode);
        }

        public async Task<IEnumerable<ShopBeerInfo>> GetNotBindedShopBeers(SourceBeerDetailsToBind sourceBeerDetails, CancellationToken cancellationToken)
        {
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress + "GetNotBindedShopBeers", sourceBeerDetails, cancellationToken);
            HttpHelper.CheckStatusCode(response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var jsonOpt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var beers = JsonSerializer.Deserialize<IEnumerable<ShopBeerInfo>>(body,jsonOpt);
            if (beers is null)
                throw new ArgumentNullException(nameof(beers));
            return beers;
        }

        public async Task<bool> TryBindShopBeers(SourceBeerDetailsToBind sourceBeerDetails, CancellationToken cancellationToken)
        {
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress + "/TryBindShopBeers", sourceBeerDetails, cancellationToken);
            HttpHelper.CheckStatusCode(response.StatusCode);
            return true;
        }
    }
}
