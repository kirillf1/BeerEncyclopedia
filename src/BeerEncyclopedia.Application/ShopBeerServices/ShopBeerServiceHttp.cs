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
            var uri = ConvertBeerQueryToUri(shopBeerQuery);
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

        private Uri ConvertBeerQueryToUri(ShopBeerQuery query)
        {
            var stringBuilder = new StringBuilder(httpClient.BaseAddress!.ToString()+"?");
            var properties = query.GetType().
                GetProperties().
                Where(x => x.CanRead).
                Where(x => x.GetValue(query, null) != null).
                ToDictionary(x => x.Name, x => x.GetValue(query, null));
            var propertyNames = properties
                .Where(x => x.Value is not string && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join("&", enumerable);
                }
            }
            stringBuilder.AppendJoin("&", properties
               .Select(x => string.Concat(
                   Uri.EscapeDataString(x.Key), "=",
                   Uri.EscapeDataString(x.Value.ToString()))));
            return new Uri(stringBuilder.ToString());
        }
    }
}
