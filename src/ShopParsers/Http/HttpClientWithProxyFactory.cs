using System.Collections.Concurrent;
using System.Net;

namespace ShopParsers.Http
{
    public class HttpClientWithProxyFactory : IHttpClientFactory
    {
        private readonly IEnumerable<ProxyContainer> proxies;
        private readonly ConcurrentQueue<ProxyContainer> proxiesQueue;
        private readonly List<string> userAgents;
        Random random;
        public HttpClientWithProxyFactory(IEnumerable<ProxyContainer> proxies, IEnumerable<string> userAgents)
        {
            this.proxies = proxies;
            proxiesQueue = new(proxies);
            this.userAgents = new List<string>(userAgents);
            random = new Random();
        }
        public HttpClient CreateHttpClient()
        {
            if (proxiesQueue.IsEmpty)
            {
                foreach (var proxyContainer in proxies.OrderBy(c => Guid.NewGuid()))
                {
                    proxiesQueue.Enqueue(proxyContainer);
                }
            }
            if (!proxiesQueue.TryDequeue(out var proxy))
            {
                return CreateHttpClient();
            }
            var webProxy = CreateWebProxy(proxy);
            var clientHandler = GetHttpHandler(proxy.ProxyType, webProxy);
            var client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
            client.Timeout = TimeSpan.FromSeconds(30);
            return client;
        }
        private static WebProxy CreateWebProxy(ProxyContainer proxyContainer)
        {
            string proxyTypeString = proxyContainer.ProxyType switch
            {
                ProxyType.SOCKS4 => "socks4://",
                ProxyType.SOCKS5 => "socks5://",
                ProxyType.HTTP => "http://",
                _ => throw new NotImplementedException(),
            };
            return new WebProxy
            {
                Address = new Uri($"{proxyTypeString}{proxyContainer.Host}:{proxyContainer.Port}")
            };
        }
        private static HttpMessageHandler GetHttpHandler(ProxyType proxyType, WebProxy webProxy)
        {
            return proxyType switch
            {
                ProxyType.SOCKS4 or ProxyType.SOCKS5 => new SocketsHttpHandler
                {
                    Proxy = webProxy
                },
                ProxyType.HTTP => new HttpClientHandler
                {
                    Proxy = webProxy
                },
                _ => throw new NotImplementedException(),
            };
        }
    }
}

