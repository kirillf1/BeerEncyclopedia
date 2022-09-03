using System.Net;

namespace ShopParsers.Http
{
    public class ProxyFactory
    {
        public static IWebProxy CreateProxy(ProxyContainer proxyContainer)
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
    }
}
