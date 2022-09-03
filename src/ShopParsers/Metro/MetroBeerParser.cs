using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ShopParsers.Metro
{
    public class MetroBeerParser : IDisposable
    {
        private readonly IWebDriver webDriver;
        private Random random;
        public MetroBeerParser(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
            random = new();
            ConfigurePage();
        }
        public async Task<IEnumerable<ShopBeer>> ParseBeers()
        {
            var beerList = new List<ShopBeer>();
            for (int i = 1; await GetBeersPerPage(i, beerList); i++) { }
            return beerList;
        }
        public async Task<IEnumerable<ShopBeer>> ParseBeers(int startPage, int endPage)
        {
            var beerList = new List<ShopBeer>();
            for (int i = startPage; i <= endPage; i++)
            {
                await GetBeersPerPage(i, beerList);
            }
            return beerList;
        }
        /// <summary>
        /// Get beers in selected page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="shopBeers"></param>
        /// <returns>If can next true</returns>
        private async Task<bool> GetBeersPerPage(int page, List<ShopBeer> shopBeers)
        {
            webDriver.Navigate().GoToUrl("https://online.metro-cc.ru/category/alkogolnaya-produkciya/pivo-sidr?order=popularity_desc&attributes=304%3Asteklo-304,zhb-304,alyuminievaya-banka-304" +
                $"&page={page}");
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            wait.Until(drv => drv.FindElement(By.XPath("//main")));
            var urls = GetDetailsUrls();
            var canNext = webDriver.FindElements(By.XPath("//ul[contains(@class,'catalog-paginate')]/li[last()]/a/*[local-name() = 'svg']")).Any();
            foreach (var url in urls)
            {
                try
                {
                    shopBeers.Add(ParseDetailsPage(url));
                    await Task.Delay(random.Next(0, 3000));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cant parse beer: " + ex.Message);
                }
            }
            return canNext;

        }
        private IEnumerable<string> GetDetailsUrls()
        {
            var elements = webDriver.FindElements(By.XPath("//main//*[@class='base-product-item__content-details']/a"));
            return elements.Select(c => c.GetAttribute("href")).ToList();
        }
        private ShopBeer ParseDetailsPage(string url)
        {
            webDriver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            wait.Until(drv => drv.FindElement(By.XPath("//h1[contains(@class,'title')]")));
            var name = webDriver.GetName();
            wait.Until(drv => drv.FindElement(By.XPath("//aside[contains(@class,'product__aside')]")));
            var priceElement = webDriver.FindElement(By.XPath("//aside[contains(@class,'product__aside')]"));
            var isAvalible = !priceElement.FindElements(By.XPath(".//div[contains(@class,'out-of-stocks')]")).Any();
            decimal price = 0;
            decimal? discountPrice = null;
            if (isAvalible)
            {
                priceElement.GetPrice(out price, out discountPrice);
            }
            wait.Until(drv => drv.FindElement(By.XPath("//section[contains(@class,'additional-block')]")));
            var brand = webDriver.GetBrand();
            var volume = webDriver.GetVolume();
            var filtration = webDriver.GetFiltration();
            var country = webDriver.GetCountry();
            var color = webDriver.GetColor();
            var pasterization = webDriver.GetPasteurization();
            double? initialWort = webDriver.GetInitialWort();

            var beer = new ShopBeer(name, price)
            {
                Brand = brand,
                Volume = volume,
                Filtration = filtration,
                Country = country,
                Color = color,
                DiscountPrice = discountPrice,
                InitialWort = initialWort,
                IsAvailable = isAvalible,
                Pasteurization = pasterization
            };
            Console.WriteLine($"added: {beer.Name} {beer.Volume} {country}, f:{filtration}, pz:{pasterization} p:{price}, av:{isAvalible} br:{brand}");
            return beer;
        }
        public void SetNewStore(int id)
        {
            webDriver.Manage().Cookies.AddCookie(new Cookie("pickupStore", id.ToString()));
        }
        private void ConfigurePage()
        {
            webDriver.Navigate().GoToUrl("https://online.metro-cc.ru");
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"__layout\"]")));
            var cookies = webDriver.Manage().Cookies;
            var ageCookie = cookies.GetCookieNamed("18ageConfirm");
            cookies.AddCookie(ChangeCookieValue(ageCookie, "true"));
            cookies.AddCookie(new Cookie("pickupStore", "11"));
            cookies.AddCookie(new Cookie("is18Confirmed", "true"));
        }
        private static Cookie ChangeCookieValue(Cookie oldCookie, string value)
        {
            return new Cookie(oldCookie.Name, value, oldCookie.Domain, oldCookie.Path,
                oldCookie.Expiry, oldCookie.Secure, oldCookie.IsHttpOnly, oldCookie.SameSite);
        }
        public void Dispose()
        {
            webDriver.Dispose();
        }
    }
}
