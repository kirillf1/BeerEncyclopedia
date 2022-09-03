using OpenQA.Selenium;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ShopParsers.Metro
{
    internal static class DetailsElementHelper
    {
        public static string GetName(this IWebDriver webDriver)
        {
            return webDriver.FindElement(By.XPath("//h1[contains(@class,'title')]")).Text;
        }
        public static bool GetPrice(this IWebDriver webDriver, out decimal price, out decimal? discountPrice)
        {
            try
            {
                var priceElement = webDriver.FindElement(By.XPath("//aside[contains(@class,'product__aside')]"));
                return GetPrice(priceElement, out price, out discountPrice);
            }
            catch
            {
                price = 0;
                discountPrice = null;
                return false;
            }
        }
        public static bool GetPrice(this IWebElement priceElement, out decimal price, out decimal? discountPrice)
        {
            try
            {
                var prices = priceElement.FindElements(By.XPath("//span[contains(@class,'price__value')]"));
                if (prices.Any())
                {
                    decimal.TryParse(Regex.Match(prices[0].Text, @"\d+([.,][0-9]{1,3})?").ValueSpan,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out price);
                    if (prices.Count > 1 &&
                        decimal.TryParse(
                            Regex.Match(prices[1].Text, @"\d+([.,][0-9]{1,3})?").ValueSpan,
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out var newDiscountPrice))
                    {
                        discountPrice = newDiscountPrice;
                        return true;
                    }
                    discountPrice = null;
                    return true;
                }
                discountPrice = null;
                price = 0;
                return false;
            }
            catch
            {
                discountPrice = null;
                price = 0;
                return false;
            }
        }
        public static string? GetBrand(this IWebDriver webDriver)
        {
            try
            {
                return webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Бренд')]]/span")).Text;
            }
            catch
            {
                return null;
            }
        }
        public static double? GetVolume(this IWebDriver webDriver)
        {
            try
            {
                var volumeString = webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Объем')]]/span")).Text;
                return double.Parse(Regex.Match(volumeString, @"\d+([.,][0-9]{1,3})?").ValueSpan);
            }
            catch
            {
                return null;
            }
        }
        public static bool? GetFiltration(this IWebDriver webDriver)
        {
            try
            {
                return webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Объем')]]/span")).Text
                    .Equals("фильтрованное", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return null;
            }
        }
        public static string? GetCountry(this IWebDriver webDriver)
        {
            try
            {
                return webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Страна')]]/span")).Text;
            }
            catch
            {
                return null;
            }
        }
        public static string? GetColor(this IWebDriver webDriver)
        {
            try
            {
                return webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Вид')]]/span")).Text;
            }
            catch
            {
                return null;
            }
        }
        public static bool? GetPasteurization(this IWebDriver webDriver)
        {
            try
            {
                return webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Пастеризация')]]/span")).Text
                .Equals("Пастеризованное", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return null;
            }
        }
        public static double? GetInitialWort(this IWebDriver webDriver)
        {
            try
            {
                var wortString = webDriver.FindElement(By.XPath("//li[div/span[contains(.,'Экстрактивность')]]/span")).Text;
                return double.Parse(wortString);
            }
            catch
            {
                return null;
            }
        }
    }
}
