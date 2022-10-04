using BeerEncyclopedia.Application.Helpers;
using BeerShared.Data;
using BeerShared.DTO;
using BeerShared.Interfaces;
using BeerShared.Queries;
using System.Collections;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BeerEncyclopedia.Application.ShopBeerServices
{
    public class ShopBeerServiceHttp : IShopBeerService
    {
        private readonly HttpClient httpClient;

        public ShopBeerServiceHttp(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new ArgumentNullException("BaseAddress is null");
            this.httpClient = httpClient;
        }
        public async Task<bool> AddBeer(ShopBeerInfo beer)
        {
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, beer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddBeers(IEnumerable<ShopBeerInfo> beerCollection)
        {
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, beerCollection);
            return response.IsSuccessStatusCode;
        }

        public async Task DeleteBeer(int id)
       {
            var response = await httpClient.DeleteAsync(httpClient.BaseAddress+ "/"+ id.ToString());
            HttpHelper.CheckStatusCode(response.StatusCode);
        }

        public async Task<ApiResult<ShopBeerInfo>> GetShopBeers(ShopBeerQuery shopBeerQuery)
        {
            var uri = HttpHelper.ConvertQueryToUri(httpClient.BaseAddress!.ToString(),shopBeerQuery);
            var response = await httpClient.GetAsync(uri);
            HttpHelper.CheckStatusCode(response.StatusCode);
            var bodyString = await response.Content.ReadAsStringAsync();
            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
            var apiResult = JsonSerializer.Deserialize<ApiResult<ShopBeerInfo>>(bodyString,opt);
            if (apiResult is null)
                throw new ArgumentNullException(nameof(apiResult));
            return apiResult;
        }

        public async Task UpdateBeer(ShopBeerInfo beer)
        {
            var response = await httpClient.PutAsJsonAsync(httpClient.BaseAddress, beer);
            HttpHelper.CheckStatusCode(response.StatusCode);
        }

       
    }
}
