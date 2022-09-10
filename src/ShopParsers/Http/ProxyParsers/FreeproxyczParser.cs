using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace ShopParsers.Http.ProxyParsers
{
    public class FreeproxyczParser : ProxyParser
    {
        public FreeproxyczParser(string cookieValue,ILogger<FreeproxyczParser> logger) : base(logger)
        {
            this.cookieValue = cookieValue;
        }
        private readonly string cookieValue;
        public override async Task<IEnumerable<ProxyContainer>> GetProxies(int count)
        {
            return await GetProxiesFromPage("http://free-proxy.cz/ru/proxylist/main/date/1");
        }
        public override async Task<IEnumerable<ProxyContainer>> GetProxies(int count, IEnumerable<string> countries)
        {
            var proxyContainers = new List<ProxyContainer>();
            foreach (var country in countries)
            {
                proxyContainers.AddRange(await GetProxiesFromPage($"http://free-proxy.cz/ru/proxylist/country/" +
                    $"{country.ToUpper()}/all/date/all/1"));
            }
            return proxyContainers;
        }

        private async Task<List<ProxyContainer>> GetProxiesFromPage(string url)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            httpClient.DefaultRequestHeaders.Add("Cookie", cookieValue);
            var proxyContainers = new List<ProxyContainer>();
            try
            {
                var html = await httpClient.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var rows = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"proxy_list\"]/tbody/tr");
                if (rows == null)
                    return proxyContainers;
                foreach (var row in rows)
                {
                    var proxyTypeString = row.SelectSingleNode(".//td[3]/small")?.InnerText;
                    if (proxyTypeString == null || !ParseProxyType(proxyTypeString, out var proxyType))
                        continue;
                    var hostRaw = row.SelectSingleNode(".//td[1]/script").InnerText;
                    var base64String = Regex.Match(hostRaw, @"\"".*\""").Value.Replace("\"", "");
                    var hostBytes = Convert.FromBase64String(base64String);
                    var host = Encoding.UTF8.GetString(hostBytes);
                    var port = row.SelectSingleNode(".//td[2]/span").InnerText;
                    proxyContainers.Add(new ProxyContainer(proxyType, host, port));
                }
            }
            catch(Exception ex)
            {
                LogErrorMessage(url, ex.Message);
            }
            return proxyContainers;
        }
    }
}
