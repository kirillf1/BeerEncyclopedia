using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopParsers.Http.ProxyParsers
{
    //TODO
    //Сделать прокси по странам
    //Добавить количество прокси серверов
    public class HidemyProxyParser : IProxyParser, IDisposable
    {
        HttpClient client;
        public HidemyProxyParser(string cookieValue)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Cookie", cookieValue);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
        }

        public async Task<IEnumerable<ProxyContainer>> GetProxies()
        {
            var proxyContainers = new List<ProxyContainer>();
            for (int i = 0; i < 300; i += 64)
            {
                proxyContainers.AddRange(await GetProxies($"https://hidemy.name/ru/proxy-list/?type=h&start{i}#list"));
            }
            return proxyContainers;
        }
        public async Task<IEnumerable<ProxyContainer>> GetProxies(IEnumerable<string> countries, int proxyCount)
        {
            var proxyContainers = new List<ProxyContainer>(proxyCount);
            var countryQuery = countries.Aggregate((f, s) => f.ToUpper() + s.ToUpper());
            for (int i = 0; i < proxyCount; i += 64)
            {
                proxyContainers.AddRange(await GetProxies($"https://hidemy.name/ru/proxy-list/?country={countryQuery}&type=h&start{i}#list"));
            }

            return proxyContainers.Take(proxyCount);
        }
        private async Task<IEnumerable<ProxyContainer>> GetProxies(string url)
        {
            var proxyContainers = new List<ProxyContainer>();

            var html = await client.GetStringAsync(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr");
            if (nodes == null || nodes.Count == 0)
                return Enumerable.Empty<ProxyContainer>();
            foreach (var row in htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr"))
            {
                var proxyTypeString = row.SelectSingleNode(".//td[5]").InnerText;
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
                var host = row.SelectSingleNode(".//td[1]").InnerText;
                var port = row.SelectSingleNode(".//td[2]").InnerText;
                proxyContainers.Add(new ProxyContainer(proxyType, host, port));
            }
            return proxyContainers;
        }
        public void Dispose()
        {
            client.Dispose();
        }

    }
}
