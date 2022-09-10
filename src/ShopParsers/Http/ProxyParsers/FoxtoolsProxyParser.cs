using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ShopParsers.Http.ProxyParsers
{
    public class FoxtoolsProxyParser : ProxyParser
    {
        public const string URL = "http://foxtools.ru/Proxy?al=True&am=True&ah=True&ahs=True&http=True&https=False";
        public FoxtoolsProxyParser(ILogger<FoxtoolsProxyParser> logger) : base(logger)
        {
        }
        public override async Task<IEnumerable<ProxyContainer>> GetProxies(int count)
        {
            return await GetProxiesFromPages(URL, count);
        }
        public override async Task<IEnumerable<ProxyContainer>> GetProxies(int count, IEnumerable<string> countries)
        {
            var proxyContainers = new List<ProxyContainer>();
            foreach (var country in countries)
            {
                proxyContainers.AddRange(await GetProxiesFromPages(URL + $"&country={country}",count));
            }
            return proxyContainers;
        }
        private async Task<List<ProxyContainer>> GetProxiesFromPages(string url, int count)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            var proxyContainers = new List<ProxyContainer>();
            try
            {
                var page = 1;
                while (count >= proxyContainers.Count)
                {
                    var html = await httpClient.GetStringAsync(URL + $"&page={page}");
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    var raws = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"theProxyList\"]/tbody/tr");
                    if (raws == null)
                        break;
                    foreach (var row in raws)
                    {
                        var host = row.SelectSingleNode(".//td[2]").InnerText;
                        var port = row.SelectSingleNode(".//td[3]").InnerText;
                        proxyContainers.Add(new ProxyContainer(ProxyType.HTTP, host, port));
                    }
                    page++;
                }
            }
            catch (Exception ex)
            {
                base.LogErrorMessage(url, ex.Message);
            }
            return proxyContainers;
        }

    }
}
