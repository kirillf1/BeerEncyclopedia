using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;


namespace ShopParsers.WineStyle
{
    public class WineStyleParser : IDisposable
    {
        private readonly HttpClient client;
        public WineStyleParser(IWebProxy webProxy)
        {
            var handler = new SocketsHttpHandler()
            {
                Proxy = webProxy
            };
            client = new HttpClient(handler);
            ConfigureHttpClient(client);
        }
        public WineStyleParser()
        {
            client = new HttpClient();
            ConfigureHttpClient(client);
        }
        public async Task<IEnumerable<ShopBeer>> ParseBeers(int start, int end, int offset = 100)
        {
            var beers = new List<ShopBeer>();

            while (start < end)
            {
                Console.WriteLine($"Parse {start}-{start + offset}");
                var html = await GetHtml(start, start + offset);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var htmlNodes = htmlDoc.DocumentNode?.SelectNodes("//form[contains(@class,'item-block')]");
                if (htmlNodes is null || !htmlNodes.Any())
                    break;
                foreach (var beerInfo in htmlNodes)
                {
                    try
                    {
                        var price = beerInfo.GetPrice();
                        var volume = beerInfo.GetVolume();
                        var title = beerInfo.GetTitle();
                        var location = beerInfo.GetLocation();
                        var manufacturer = beerInfo.GetManufacturer();
                        var beerColumn = beerInfo.GetBeerColumn();
                        var detailsUrl = beerInfo.GetDetailsUrl();
                        var strenght = beerInfo.GetStrength();
                        var style = beerInfo.GetStyle() ?? "unknown";
                        var rating = beerInfo.GetRating();
                        var brand = beerInfo.GetBrand();
                        var isAvalible = beerInfo.IsAvailable();
                        beers.Add(new ShopBeer(title, price)
                        {
                            Volume = volume,
                            Country = location.Country,
                            Manufacturer = manufacturer,
                            Color = beerColumn.Color,
                            Filtration = beerColumn.Filtraion,
                            Pasteurization = beerColumn.Pasteurization,
                            DetailsUrl = detailsUrl,
                            Brand = brand,
                            Rating = rating,
                            Strength = strenght,
                            Style = style,
                            IsAvailable = isAvalible
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Parsing beer failed. Message: {ex.Message}");
                        continue;
                    }
                }
                start += offset;
            }
            return beers;
        }
        private async Task<string> GetHtml(int start, int end)
        {
            var url = "https://winestyle.ru//remote.php?r=0.3391295572713491&w=loadcatalogproducts&nodesurl=%2Fbeer%2F250ml%2F300ml%2F320ml%2F330ml%2F333ml%2F350ml%2F355ml%2F375ml%2F400ml%2F410ml%2F425ml%2F430ml%2F440ml%2F450ml%2F460ml%2F470ml%2F473ml%2F480ml%2F490ml%2F500ml%2F550ml%2F568ml%2F580ml%2F600ml%2F610ml%2F620ml%2F630ml%2F640ml%2F650ml%2F660ml%2F700ml%2F710ml%2F750ml_ll%2F&sort=popularityASC&" +
        $"start={start}&end={end}&capacityfilter=0&group_tab_id=0";

            Console.WriteLine($"{start} - {end} started");
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var code = response.StatusCode;
                throw new WebException($"Server rejected request with code {(int)code} winestyleRequest: {start}-{start + end}");
            }
            var resStr = await response.Content.ReadAsStringAsync();
            var html = Regex.Unescape(resStr);
            return html;
        }

        private static void ConfigureHttpClient(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            httpClient.Timeout = TimeSpan.FromSeconds(40);
        }

        public void Dispose() => client.Dispose();
    }
}

