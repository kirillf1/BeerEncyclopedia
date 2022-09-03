using HtmlAgilityPack;

namespace ShopParsers.Http.ProxyParsers
{
    public class FoxtoolsProxyParser : IProxyParser
    {
        public async Task<IEnumerable<ProxyContainer>> GetProxies()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            var proxyContainers = new List<ProxyContainer>();
            for (int i = 1; i <= 4; i++)
            {
                try
                {
                    var html = await httpClient.GetStringAsync($"http://foxtools.ru/Proxy?al=True&am=True&ah=True&ahs=True&http=True&https=False&page={i}");
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    var raws = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"theProxyList\"]/tbody/tr");
                    foreach (var row in raws)
                    {
                        var host = row.SelectSingleNode(".//td[2]").InnerText;
                        var port = row.SelectSingleNode(".//td[3]").InnerText;
                        proxyContainers.Add(new ProxyContainer(ProxyType.HTTP, host, port));
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return proxyContainers;
        }
    }
}
