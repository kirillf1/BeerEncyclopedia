using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace ShopParsers.Perekrestok
{
    ////https://www.perekrestok.ru/api/customer/1.4.1.0/catalog/product/plu3350097 -plu хз че, далее id
    ////https://www.perekrestok.ru/api/customer/1.4.1.0/shop/points - карта точек
    ////https://www.perekrestok.ru/api/customer/1.4.1.0/shop/605 - получаем инфу о магазине с точки
    ////https://www.perekrestok.ru/api/customer/1.4.1.0/delivery/mode/pickup/628 - put чтобы изменить локацию
    public class PerekrestokParser : IDisposable
    {
        public const string URL = "https://www.perekrestok.ru";
        private HttpClient httpClient;
        private SocketsHttpHandler clientHandler;
        private const int categoryId = 9;
        private bool isAuthenticate;
        public PerekrestokParser()
        {
            clientHandler = new SocketsHttpHandler();
            httpClient = new HttpClient(clientHandler);
            ConfigureHeaders(httpClient);
        }
        public PerekrestokParser(IWebProxy webProxy)
        {
            clientHandler = new SocketsHttpHandler()
            {
                Proxy = webProxy
            };
            httpClient = new HttpClient(clientHandler);
            ConfigureHeaders(httpClient);
        }
        public async Task<IEnumerable<ShopBeer>> ParseBeers()
        {
            if (!isAuthenticate)
                await Authenticate();
            var query = new PerekrestokQuery()
            {
                Page = 1,
                PerPage = 48,
                Filter = new Filter() { Category = categoryId }
            };
            var content = new StringContent(JsonSerializer.Serialize(query).ToLower(),
                Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://www.perekrestok.ru/api/customer/1.4.1.0/catalog/product/grouped-feed", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Getting beers failed with code {(int)response.StatusCode}");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            var dataModel = JsonSerializer.Deserialize<PerekrestokPreviewModel>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (dataModel == null)
                return Enumerable.Empty<ShopBeer>();
            var beers = new List<ShopBeer>();
            foreach (var item in dataModel.Content.Items.SelectMany(c => c.Products))
            {
                beers.Add(CreateBeer(item));
            }
            return beers;
        }
        private async Task ChangeLocation(double ln, double lt)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            clientHandler.Dispose();
            httpClient.Dispose();
        }
        private async Task Authenticate()
        {
            clientHandler.CookieContainer.Add(new Uri(URL), new Cookie("agreements", HttpUtility.UrlEncode("j:{\"isCookieAccepted\":false,\"isAdultContentEnabled\":true,\"isAppAppInstallPromptClosed\":false}")));
            var emptyResponse = await httpClient.GetAsync(URL);
            if (!emptyResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Connection to perekrestok failed with code {(int)emptyResponse.StatusCode}");
            }
            var cookies = clientHandler.CookieContainer
                               .GetCookies(new Uri(URL));
            var sessionCookie = cookies.First(c => c.Name == "session");
            var sessionValue = HttpUtility.UrlDecode(sessionCookie.Value);
            httpClient.DefaultRequestHeaders.Add("Auth", $"Bearer {GetBearerFromSession(sessionValue)}");
            isAuthenticate = true;
        }
        private static void ConfigureHeaders(HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"104\", \" Not A;Brand\";v=\"99\", \"Google Chrome\";v=\"104\"");
            httpClient.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Origin", URL);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            httpClient.DefaultRequestHeaders.Add("Host", "www.perekrestok.ru");
        }
        private static string GetBearerFromSession(string sessionValue)
        {
            var pattern = "\"accessToken\":\"";
            var bearerStartIndex = sessionValue.IndexOf(pattern) + pattern.Length;
            var bearerEndIndex = sessionValue.IndexOf("\"", bearerStartIndex + pattern.Length);
            var bearer = sessionValue[bearerStartIndex..bearerEndIndex];
            return bearer;
        }
        private static ShopBeer CreateBeer(Product product)
        {
            return new ShopBeer(product.Title, product.PriceTag?.GrossPrice / 100 ?? product.MedianPrice / 100)
            {
                DetailsUrl = $"https://www.perekrestok.ru/api/customer/1.4.1.0/catalog/product/plu{product.MasterData.Plu}",
                DiscountPrice = product.PriceTag?.Price / 100,
                Rating = product.Rating / 100,
                Volume = product.MasterData.Volume / 100,
                IsAvailable = product.BalanceState.Equals("many", StringComparison.OrdinalIgnoreCase)
            };
        }
    }
}
