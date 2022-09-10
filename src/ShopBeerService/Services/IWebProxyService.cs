using System.Net;

namespace ShopBeerService.Services
{
    public interface IWebProxyService
    {
        Task<IEnumerable<IWebProxy>> GetProxies(int count);
        Task<IEnumerable<IWebProxy>> GetProxiesByCountry(int count, string countryShortName);
        Task<IWebProxy?> GetWorkingProxy(string targetUrl, string countryShortName, int checksCount = 45);
    }
}