using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace ShopParsers.Http.ProxyParsers
{
    public class FreeproxyczParser : IProxyParser
    {
        public async Task<IEnumerable<ProxyContainer>> GetProxies()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            httpClient.DefaultRequestHeaders.Add("Cookie", "fp=574b322b1c60aa0824557880a46eee1c");
            var proxyContainers = new List<ProxyContainer>();
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    var html = await httpClient.GetStringAsync($"http://free-proxy.cz/ru/proxylist/main/date/{i}");
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    var raws = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"proxy_list\"]/tbody/tr");
                    foreach (var row in raws)
                    {
                        var proxyTypeString = row.SelectSingleNode(".//td[3]/small")?.InnerText;
                        if (proxyTypeString == null)
                            continue;
                        ProxyType proxyType;
                        if (proxyTypeString.Equals("HTTP", StringComparison.OrdinalIgnoreCase))
                            proxyType = ProxyType.HTTP;
                        else if (proxyTypeString.Equals("SOCKS4", StringComparison.OrdinalIgnoreCase))
                            proxyType = ProxyType.SOCKS4;
                        else if (proxyTypeString.Equals("SOCKS5", StringComparison.OrdinalIgnoreCase))
                            proxyType = ProxyType.SOCKS5;
                        else
                            continue;
                        var hostRaw = row.SelectSingleNode(".//td[1]/script").InnerText;
                        var base64String = Regex.Match(hostRaw, @"\"".*\""").Value.Replace("\"", "");
                        var hostBytes = Convert.FromBase64String(base64String);
                        var host = Encoding.UTF8.GetString(hostBytes);
                        var port = row.SelectSingleNode(".//td[2]/span").InnerText;
                        proxyContainers.Add(new ProxyContainer(proxyType, host, port));
                    }
                }
                catch
                {
                    continue;
                }
            }
            return proxyContainers;
        }
    }
}
