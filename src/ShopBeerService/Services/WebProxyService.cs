using ShopParsers.Http;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;

namespace ShopBeerService.Services
{
    public class WebProxyService : IWebProxyService
    {
        public WebProxyService(IEnumerable<ProxyParser> proxyParsers)
        {
            this.proxyParsers = new ReadOnlyCollection<ProxyParser>(proxyParsers.ToList());
            proxyCountryChache = new(StringComparer.OrdinalIgnoreCase);
        }
        private const string AllCountriesKey = "ALL";
        private Dictionary<string, Queue<IWebProxy>> proxyCountryChache;
        private IReadOnlyCollection<ProxyParser> proxyParsers;
        public async Task<IEnumerable<IWebProxy>> GetProxies(int count)
        {
            return await GetProxiesUntilCount(count,
                p => p.GetProxies(count));
        }
        public async Task<IEnumerable<IWebProxy>> GetProxiesByCountry(int count, string countryShortName)
        {
            if (countryShortName.Equals(AllCountriesKey, StringComparison.OrdinalIgnoreCase))
                return await GetProxies(count);
            return await GetProxiesUntilCount(count,
                p => p.GetProxies(count, new string[] { countryShortName }));
        }
        public async Task<IWebProxy?> GetWorkingProxy(string targetUrl, string countryShortName, int checksCount = 45)
        {
            IWebProxy? wokingProxy = default;
            try
            {
                var handler = new SocketsHttpHandler();
                using HttpClient client = new(handler);
                client.Timeout = TimeSpan.FromSeconds(30);
                proxyCountryChache.TryGetValue(countryShortName, out var webProxiesQueue);
                for (int i = 0; i < checksCount; i++)
                {
                    if (webProxiesQueue == null || webProxiesQueue.Count == 0)
                        webProxiesQueue = await EnqueueProxies(countryShortName, checksCount);
                    wokingProxy = webProxiesQueue.Dequeue();
                    if (await CheckProxyWorking(targetUrl, client))
                        break;
                    wokingProxy = default;
                }
            }
            catch { }
            return wokingProxy;
        }
        private async Task<Queue<IWebProxy>> EnqueueProxies(string key, int proxyCount)
        {
            Queue<IWebProxy> proxyQueue = new(await GetProxiesByCountry(proxyCount, key));
            proxyCountryChache[key] = proxyQueue;
            return proxyQueue;
        }
        private static async Task<bool> CheckProxyWorking(string url, HttpClient httpClient)
        {
            try
            {
                var result = await httpClient.GetAsync(url);
                return result.IsSuccessStatusCode;
            }
            catch { return false; }
        }
        private async Task<IEnumerable<IWebProxy>> GetProxiesUntilCount(int count,
            Func<ProxyParser, Task<IEnumerable<ProxyContainer>>> proxyParserMethod)
        {
            var proxiesList = new List<IWebProxy>();
            foreach (var proxyParser in proxyParsers.OrderBy(c => Guid.NewGuid()))
            {
                var pContainers = await proxyParserMethod(proxyParser);
                proxiesList.AddRange(pContainers.Select(c => c.CreateWebProxy()).OrderBy(c=>Guid.NewGuid()));
                if (proxiesList.Count >= count)
                    break;
            }
            return proxiesList;
        }
    }
}
