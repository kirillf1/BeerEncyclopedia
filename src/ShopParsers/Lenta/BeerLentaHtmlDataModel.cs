#nullable disable
using Newtonsoft.Json;

namespace ShopParsers.Lenta
{
    internal class BeerLentaHtmlDataModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("subTitle")]
        public string SubTitle { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("regularPrice")]
        public RegularPrice RegularPrice { get; set; }

        [JsonProperty("cardPrice")]
        public CardPrice CardPrice { get; set; }

        [JsonProperty("isShowOnePrice")]
        public bool IsShowOnePrice { get; set; }

        [JsonProperty("skuUrl")]
        public string SkuUrl { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("promotionType")]
        public int PromotionType { get; set; }

        [JsonProperty("promoPercent")]
        public int PromoPercent { get; set; }

        [JsonProperty("promoStart")]
        public DateTime PromoStart { get; set; }

        [JsonProperty("promoEnd")]
        public DateTime PromoEnd { get; set; }

        [JsonProperty("hasDiscount")]
        public bool HasDiscount { get; set; }

        [JsonProperty("isPromoForRegularPrice")]
        public bool IsPromoForRegularPrice { get; set; }

        [JsonProperty("isWeightProduct")]
        public bool IsWeightProduct { get; set; }

        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("commentsCount")]
        public int CommentsCount { get; set; }

        [JsonProperty("gaCategory")]
        public string GaCategory { get; set; }

        [JsonProperty("badges")]
        public List<object> Badges { get; set; }

        [JsonProperty("isEcomEnabled")]
        public bool IsEcomEnabled { get; set; }

        [JsonProperty("weightOptionsMax")]
        public List<object> WeightOptionsMax { get; set; }

        [JsonProperty("defaultSelectedWeightOptionIndex")]
        public int DefaultSelectedWeightOptionIndex { get; set; }

        [JsonProperty("kppPageUrl")]
        public string KppPageUrl { get; set; }

        [JsonProperty("placeOutput")]
        public string PlaceOutput { get; set; }

        [JsonProperty("isInFavorites")]
        public bool IsInFavorites { get; set; }

        [JsonProperty("isAvailableForOrder")]
        public bool IsAvailableForOrder { get; set; }

        [JsonProperty("stock")]
        public int Stock { get; set; }

        [JsonProperty("stockValue")]
        public string StockValue { get; set; }

        [JsonProperty("isAvailableInStore")]
        public bool IsAvailableInStore { get; set; }

        [JsonProperty("isDeliveryEnabled")]
        public bool IsDeliveryEnabled { get; set; }

        [JsonProperty("preventIndexing")]
        public bool PreventIndexing { get; set; }

        [JsonProperty("isShowDeliveryUnavailableError")]
        public bool IsShowDeliveryUnavailableError { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("showUnavailableToOrderForProError")]
        public bool ShowUnavailableToOrderForProError { get; set; }

        [JsonProperty("isShowEcomCartControl")]
        public bool IsShowEcomCartControl { get; set; }

        [JsonProperty("hasPrices")]
        public bool HasPrices { get; set; }

        [JsonProperty("unavailableToOrderForProErrorMessage")]
        public string UnavailableToOrderForProErrorMessage { get; set; }

        [JsonProperty("hasAdultContent")]
        public bool HasAdultContent { get; set; }

        [JsonProperty("priceWithoutNds")]
        public PriceWithoutNds PriceWithoutNds { get; set; }

        [JsonProperty("proProductType")]
        public string ProProductType { get; set; }

        [JsonProperty("isProCardSelected")]
        public bool IsProCardSelected { get; set; }
    }
    public class CardPrice
    {
        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("integerPart")]
        public string IntegerPart { get; set; }

        [JsonProperty("fractionPart")]
        public string FractionPart { get; set; }
    }

    public class PriceWithoutNds
    {
        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("integerPart")]
        public string IntegerPart { get; set; }

        [JsonProperty("fractionPart")]
        public string FractionPart { get; set; }
    }

    public class RegularPrice
    {
        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("integerPart")]
        public string IntegerPart { get; set; }

        [JsonProperty("fractionPart")]
        public string FractionPart { get; set; }
    }

}
