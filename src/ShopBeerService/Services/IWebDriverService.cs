using OpenQA.Selenium;

namespace ShopBeerService.Services
{
    public record WebDriverContainer : IDisposable
    {
        public WebDriverContainer(IEnumerable<IWebDriver> webDrivers)
        {
            WebDrivers = new List<IWebDriver>(webDrivers);
        }
        public IReadOnlyCollection<IWebDriver> WebDrivers { get; }

        public void Dispose()
        {
            foreach (var webDriver in WebDrivers)
            {
                webDriver.Dispose();
            }
        }
    }
    public interface IWebDriverService
    {
        public WebDriverContainer GetConfiguredWebDrivers(int count);
    }
}
