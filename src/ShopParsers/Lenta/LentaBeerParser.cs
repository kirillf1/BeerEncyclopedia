using HtmlAgilityPack;
using System.Net;
using System.Text.Json;
using System.Web;

namespace ShopParsers.Lenta
{
    //https://lenta.com//api/v2/cities/msk/stores магазины
    // /api/v1/cities 
    ///api/v1/search?value= - поиск по названию
    public class LentaBeerParser : IDisposable
    {
        private const string lentaBeerUrl = "https://lenta.com/catalog/alkogolnye-napitki/pivo-i-slabyjj-alkogol";
        private static readonly Dictionary<LentaBeerCategory, string> categoryPaths;
        static LentaBeerParser()
        {
            categoryPaths = new Dictionary<LentaBeerCategory, string>();
            categoryPaths[LentaBeerCategory.NonAlcoholic] = "bezalkogolnoe-pivo";
            categoryPaths[LentaBeerCategory.License] = "licenzionnoe-pivo";
            categoryPaths[LentaBeerCategory.Import] = "importnoe-pivo";
            categoryPaths[LentaBeerCategory.Craft] = "kraftovoe-pivo";
            categoryPaths[LentaBeerCategory.Domestic] = "otechestvennoe-pivo";
            categoryPaths[LentaBeerCategory.Taste] = "vkusovoe-pivo";

        }
        private readonly HttpClient httpClient;
        private readonly SocketsHttpHandler httpHandler;
        private Random random = new Random();
        public LentaBeerParser(string shopId = "0302")
        {
            httpHandler = new SocketsHttpHandler();
            ConfigureCookie(httpHandler, shopId);
            httpClient = new HttpClient(httpHandler);
            AddHeaders(httpClient);
        }
        /// <summary>
        /// Init with proxy. Note requests will only be processed from a Russian IP
        /// </summary>
        /// <param name="shopId"></param>
        public LentaBeerParser(IWebProxy webProxy, string shopId = "0302")
        {
            httpHandler = new SocketsHttpHandler()
            {
                Proxy = webProxy
            };
            ConfigureCookie(httpHandler, shopId);
            httpClient = new HttpClient(httpHandler);
            AddHeaders(httpClient);
        }
        public async Task<IEnumerable<ShopBeer>> ParseBeers(IEnumerable<LentaBeerCategory> lentaBeerCategories)
        {
            var beers = new List<ShopBeer>();
            foreach (var category in lentaBeerCategories)
            {
                var path = categoryPaths[category];
                bool canNext = true;
                var page = 1;
                while (canNext)
                {
                    canNext = await ParsePerPage(beers, $"{lentaBeerUrl}/{path}/?page={page}");
                    page++;
                    await Task.Delay(random.Next(5000, 10000));
                }
            }
            return beers;
        }
        public async Task<IEnumerable<ShopBeer>> ParseBeers(LentaBeerCategory lentaBeerCategory, int page)
        {
            var beers = new List<ShopBeer>();
            var path = categoryPaths[lentaBeerCategory];
            await ParsePerPage(beers, $"{lentaBeerUrl}/{path}/?page={page}");
            return beers;
        }
        public async Task<int> GetPageCount(LentaBeerCategory lentaBeerCategory)
        {
            var path = categoryPaths[lentaBeerCategory];
            var html = await GetLentaShopHtml($"{lentaBeerUrl}/{path}");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            if (string.IsNullOrEmpty(html))
                return 0;
            var pageString = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='pagination__item'][last()]/a").InnerText;
            int.TryParse(pageString, out var page);
            return page;
        }
        private async Task<bool> ParsePerPage(List<ShopBeer> beers, string url)
        {
            var html = await GetLentaShopHtml(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            if (string.IsNullOrEmpty(html))
                return false;
            beers.AddRange(ParseByCategory(htmlDoc.DocumentNode));
            var nextNode = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='next']");
            return nextNode is not null;
        }
        private async Task<string> GetLentaShopHtml(string url)
        {
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                await Task.Delay(3000);
                response = await httpClient.GetAsync(url);
            }
            if (!response.IsSuccessStatusCode)
                throw new Exception("error with code " + response.StatusCode.ToString());
            return await response.Content.ReadAsStringAsync();
        }
        private static IEnumerable<ShopBeer> ParseByCategory(HtmlNode html)
        {
            var beersRaw = html.SelectNodes("//div[contains(@class,'sku-card-small-container')]")?.
                Select(c => HttpUtility.HtmlDecode(
                    c.Attributes.First(a => a.Name == "data-model").Value));
            if (beersRaw == null || !beersRaw.Any())
                return Enumerable.Empty<ShopBeer>();
            var beers = new List<ShopBeer>();
            foreach (var beerRaw in beersRaw)
            {
                var beerDataModel = JsonSerializer.Deserialize<BeerLentaHtmlDataModel>(beerRaw, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (beerDataModel == null)
                    continue;
                beers.Add(CreateBeer(beerDataModel));
            }
            return beers;
        }
        private static ShopBeer CreateBeer(BeerLentaHtmlDataModel dataModel)
        {
            return new ShopBeer(dataModel.Title, (decimal)dataModel.RegularPrice.Value)
            {
                DetailsUrl = lentaBeerUrl + dataModel.SkuUrl,
                DiscountPrice = (decimal)dataModel.CardPrice.Value,
            };
        }
        public static void AddHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"104\", \" Not A;Brand\";v=\"99\", \"Google Chrome\";v=\"104\"");
            httpClient.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Origin", "https://lenta.com");
            httpClient.Timeout = TimeSpan.FromSeconds(30);
        }
        private static void ConfigureCookie(SocketsHttpHandler handler, string shopId)
        {
            handler.CookieContainer.Add(new Uri("https://lenta.com"), new Cookie("Store", shopId));
            handler.CookieContainer.Add(new Uri("https://lenta.com"), new Cookie("DeliveryOptions", "Pickup"));
            handler.CookieContainer.Add(new Uri("https://lenta.com"), new Cookie("ShouldSetDeliveryOptions", "False"));
            handler.CookieContainer.Add(new Uri("https://lenta.com"), new Cookie("IsAdult", "True"));
            handler.CookieContainer.Add(new Uri("https://lenta.com"), new Cookie("CityCookie", "true"));
            handler.CookieContainer.Add(new Uri("https://lenta.com"), new Cookie("DontShowCookieNotification", "true"));
        }

        public void Dispose()
        {
            httpHandler.Dispose();
            httpClient.Dispose();
        }
    }
}
