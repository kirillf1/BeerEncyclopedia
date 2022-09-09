using System.Net;

namespace ShopParsers.Http
{
    public class ProxyContainer
    {
        public ProxyContainer(ProxyType proxyType, string host, string port)
        {
            ProxyType = proxyType;
            Host = host;
            Port = port;
        }
        public IWebProxy CreateWebProxy()
        {
            return ProxyFactory.CreateProxy(this);
        }
        public ProxyType ProxyType { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }

    }
}
