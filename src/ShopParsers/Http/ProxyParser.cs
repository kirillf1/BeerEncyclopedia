using Microsoft.Extensions.Logging;

namespace ShopParsers.Http
{
    public abstract class ProxyParser
    {
        static ProxyParser()
        {
            ProxyTypes = new(StringComparer.OrdinalIgnoreCase);
            ProxyTypes["HTTP"] = ProxyType.HTTP;
            ProxyTypes["SOCKS4"] = ProxyType.SOCKS4;
            ProxyTypes["SOCKS5"] = ProxyType.SOCKS5;
        }
        private static Dictionary<string, ProxyType> ProxyTypes;
        public ProxyParser(ILogger logger)
        {
            this.logger = logger;
        }
        protected readonly ILogger logger;

        public abstract Task<IEnumerable<ProxyContainer>> GetProxies(int count);
        public abstract Task<IEnumerable<ProxyContainer>> GetProxies(int count, IEnumerable<string> countries);
        protected void LogErrorMessage(string url, string errorMessage)
        {
            logger.LogError("Can't parse proxies from url: {url}. Error message: {error}", url,errorMessage);
        }
        protected static bool ParseProxyType(string proxyTypeString,out ProxyType proxyType)
        {
            return ProxyTypes.TryGetValue(proxyTypeString, out proxyType);
        }
    }
}
