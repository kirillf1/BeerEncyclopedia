namespace ShopParsers.Http
{
    public interface IProxyParser
    {
        public Task<IEnumerable<ProxyContainer>> GetProxies();
    }
}
