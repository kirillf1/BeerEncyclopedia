using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ShopParsers.Http.ProxyParsers
{
    public class HidemyProxyParser : ProxyParser
    {
        public HidemyProxyParser(string cookieValue, ILogger<HidemyProxyParser> logger) : base(logger)
        {
            this.cookieValue = cookieValue;
        }
        private const int MaxProxiesInPage = 45;  
        private readonly string cookieValue;       
        public async override Task<IEnumerable<ProxyContainer>> GetProxies(int count)
        {
            return await GetProxiesFromPages("https://hidemy.name/ru/proxy-list/?type=h45", count);
        }
        public override async Task<IEnumerable<ProxyContainer>> GetProxies(int count, IEnumerable<string> countries)
        {
            var countryQuery = countries.Aggregate((f, s) => f.ToUpper() + s.ToUpper());
            return await GetProxiesFromPages($"https://hidemy.name/ru/proxy-list/?country={countryQuery}&type=h45",count);       
        }
        private async Task<IEnumerable<ProxyContainer>> GetProxiesFromPages(string url,int count)
        {
            var proxyContainers = new List<ProxyContainer>(count);
            var pageCount = 0;
            while(count > proxyContainers.Count)
            {
                var proxies = await GetProxiesFromPage(url+$"&start={pageCount}#list");
                if (!proxies.Any())
                    break;
                proxyContainers.AddRange(proxies);
                pageCount += MaxProxiesInPage;
            }
            return proxyContainers.Take(count);
        }
        private async Task<IEnumerable<ProxyContainer>> GetProxiesFromPage(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Cookie", cookieValue);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            var proxyContainers = new List<ProxyContainer>();
            try
            {
                var html = await client.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var nodes = htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr");
                if (nodes == null || nodes.Count == 0)
                    return Enumerable.Empty<ProxyContainer>();
                foreach (var row in htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr"))
                {
                    var proxyTypeString = row.SelectSingleNode(".//td[5]").InnerText;
                    if (!ParseProxyType(proxyTypeString, out var proxyType))
                        continue;
                    var host = row.SelectSingleNode(".//td[1]").InnerText;
                    var port = row.SelectSingleNode(".//td[2]").InnerText;
                    proxyContainers.Add(new ProxyContainer(proxyType, host, port));
                }
            }
            catch(Exception e)
            {
                LogErrorMessage(url, e.Message);
            }
            return proxyContainers;
        }
    }
}
