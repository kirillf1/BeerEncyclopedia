using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ShopParsers.WineStyle
{
    public static class WineStyleHelper
    {
        public static string GetTitle(this HtmlNode htmlNode)
        {
            //return ConvertRawTitle(htmlNode.SelectSingleNode(".//a[contains(@itemprop,'url')]").InnerText);
            return htmlNode.SelectSingleNode(".//a[contains(@itemprop,'url')]").InnerText;
        }
        public record Location(string Country, string? Region);
        public static Location GetLocation(this HtmlNode htmlNode)
        {
            var locationRaw = htmlNode.
                SelectSingleNode(".//li[contains(.,'Регион')]").
                SelectNodes(".//a").
                Select(c => c.InnerText).ToArray();
            var country = locationRaw[0];
            var region = locationRaw.Length >= 2 ? locationRaw[1] : null;
            return new Location(country, region);
        }
        public static double GetRating(this HtmlNode htmlNode)
        {
            var ratingRaw = htmlNode.
                 SelectSingleNode(".//div[contains(@class,'info-block rating-text')]")?.
                 SelectSingleNode(".//span[@class='text']")?.InnerHtml;
            if (ratingRaw == null)
                return 0;
            NumberFormatInfo loNumberFormatInfo = new();
            loNumberFormatInfo.NumberDecimalSeparator = ".";
            return double.Parse(Regex.Match(ratingRaw, @"\d+([.,][0-9]{1,3})?").ValueSpan, NumberStyles.Any, loNumberFormatInfo);
        }
        public static bool IsAvailable(this HtmlNode htmlNode)
        {
            return htmlNode.
               SelectSingleNode(".//div[contains(.,'Нет в наличии')]") is null;
        }
        public static string? GetBrand(this HtmlNode htmlNode)
        {
            return htmlNode.
               SelectSingleNode(".//li[contains(.,'Бренд')]")?.
               SelectSingleNode(".//a")?.InnerText;
        }
        public static string GetDetailsUrl(this HtmlNode htmlNode)
        {
            return "https://winestyle.ru" + htmlNode.
                SelectSingleNode(".//a[@itemprop='url']").
                Attributes.First(c => c.Name == "href").Value;
        }
        public record BeerColumn(string Color, bool Filtraion, bool Pasteurization);
        public static BeerColumn GetBeerColumn(this HtmlNode htmlNode)
        {
            var columnRaw = htmlNode.
              SelectSingleNode(".//li[contains(.,'Пиво')]").
              SelectNodes(".//a").
              Select(c => c.InnerText).ToArray();
            var color = columnRaw[0];
            var filtration = columnRaw.Skip(1).Any(c => c.Equals("фильтрованное", StringComparison.OrdinalIgnoreCase));
            var pasterilisation = !columnRaw.Skip(1).Any(c => c.Equals("Живое", StringComparison.OrdinalIgnoreCase));
            return new BeerColumn(color, filtration, pasterilisation);
        }
        public static string GetManufacturer(this HtmlNode htmlNode)
        {
            return htmlNode.
               SelectSingleNode(".//li[contains(.,'Производитель')]")
               .SelectNodes(".//a").First().InnerText;
        }
        public static string? GetStyle(this HtmlNode htmlNode)
        {
            return htmlNode.
             SelectSingleNode(".//li[contains(.,'Стиль')]")?.
             SelectNodes(".//a")?.Last()?.InnerText;
        }
        public static double GetStrength(this HtmlNode htmlNode)
        {
            var rawStrenght = htmlNode.
              SelectSingleNode(".//li[contains(.,'Крепость')]").InnerText;
            NumberFormatInfo loNumberFormatInfo = new();
            loNumberFormatInfo.NumberDecimalSeparator = ".";
            return double.Parse(Regex.Match(rawStrenght, @"\d+([.,][0-9]{1,3})?").ValueSpan, NumberStyles.Any, loNumberFormatInfo);

        }
        public static decimal GetPrice(this HtmlNode htmlNode)
        {
            var priceString = htmlNode.SelectSingleNode(".//div[@class='price ']").InnerText;
            return decimal.Parse(Regex.Match(priceString, @"\d+([.,][0-9]{1,3})?").ValueSpan);
        }
        private static string ConvertRawTitle(string title)
        {
            return Regex.Replace(title.Replace("\"", "").
                Replace(",", ""), @"\s*(in can|в жестяной банке)?\s*\d+([.,][0-9]{1,3})?\s*(мл|л)", "");
        }
        public static double GetVolume(this HtmlNode htmlNode)
        {
            var volumeNodes = htmlNode.SelectNodes(".//div[contains(@class,'radio-container ')]");
            double? currentVolume = null;
            foreach (var volumeHode in volumeNodes)
            {
                var volumeNodeString = volumeHode.InnerText;
                NumberFormatInfo loNumberFormatInfo = new();
                loNumberFormatInfo.NumberDecimalSeparator = ".";
                if (!double.TryParse(Regex.Match(volumeNodeString, @"\d+([.,][0-9]{1,3})?").ValueSpan,
                    NumberStyles.Any, loNumberFormatInfo, out var volume))
                    continue;
                if (volumeNodeString.Contains("мл"))
                    volume /= 1000;
                if (!currentVolume.HasValue)
                {
                    currentVolume = volume;
                    if (currentVolume == 0.5)
                        break;
                    continue;
                }
                if (Math.Abs(currentVolume.Value - 0.5) > Math.Abs(volume - 0.5))
                    currentVolume = volume;
            }
            if (!currentVolume.HasValue)
                throw new ArgumentNullException();
            return currentVolume.Value;

        }
    }
}
