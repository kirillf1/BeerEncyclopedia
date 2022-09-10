using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ShopBeerService.Services
{
    public class ChromeDriverService : IWebDriverService
    {
        private readonly string driverPath;

        public ChromeDriverService(string driverPath)
        {
            this.driverPath = driverPath;
        }
        public WebDriverContainer GetConfiguredWebDrivers(int count)
        {
            var drivers = new List<IWebDriver>();
            for (int i = 0; i < count; i++)
            {
                drivers.Add(new ChromeDriver(driverPath));   
            }
            return new WebDriverContainer(drivers);
        }
    }
}
