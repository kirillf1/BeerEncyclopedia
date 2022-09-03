using Newtonsoft.Json;
#nullable disable
namespace ShopParsers.Perekrestok
{
    public class PerekrestokPreviewModel
    {
        [JsonProperty("content")]
        public Content Content { get; set; }
    }
    public class Content
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("currentSorting")]
        public CurrentSorting CurrentSorting { get; set; }

        [JsonProperty("loyaltyClub")]
        public object LoyaltyClub { get; set; }
    }

    public class CurrentSorting
    {
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }

        [JsonProperty("orderDirection")]
        public string OrderDirection { get; set; }
    }

    public class Group
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class Image
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("originName")]
        public string OriginName { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileSize")]
        public int FileSize { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("cropUrlTemplate")]
        public string CropUrlTemplate { get; set; }

        [JsonProperty("publicUrl")]
        public object PublicUrl { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }

    public class Item
    {
        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }

    public class Label
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class MasterData
    {
        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("plu")]
        public string Plu { get; set; }

        [JsonProperty("unit")]
        public int Unit { get; set; }

        [JsonProperty("unitName")]
        public string UnitName { get; set; }

        [JsonProperty("quantum")]
        public int Quantum { get; set; }

        [JsonProperty("quantumStep")]
        public int QuantumStep { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("privateLabel")]
        public bool PrivateLabel { get; set; }
    }

    public class PriceTag
    {
        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("discountMinimumAmount")]
        public int DiscountMinimumAmount { get; set; }

        [JsonProperty("grossPrice")]
        public int? GrossPrice { get; set; }

        [JsonProperty("labels")]
        public List<Label> Labels { get; set; }

        [JsonProperty("pricePerQuantum")]
        public object PricePerQuantum { get; set; }

        [JsonProperty("grossPricePerQuantum")]
        public object GrossPricePerQuantum { get; set; }
    }

    public class PrimaryCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("isAlcohol")]
        public bool IsAlcohol { get; set; }

        [JsonProperty("isTobacco")]
        public bool IsTobacco { get; set; }

        [JsonProperty("isAdultContent")]
        public bool IsAdultContent { get; set; }

        [JsonProperty("displayStyle")]
        public int DisplayStyle { get; set; }

        [JsonProperty("catalogs")]
        public List<int> Catalogs { get; set; }
    }

    public class Product
    {
        [JsonProperty("enlargedCard")]
        public bool EnlargedCard { get; set; }

        [JsonProperty("liked")]
        public bool Liked { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("masterData")]
        public MasterData MasterData { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("primaryCategory")]
        public PrimaryCategory PrimaryCategory { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("borderStyle")]
        public object BorderStyle { get; set; }

        [JsonProperty("medianPrice")]
        public int MedianPrice { get; set; }

        [JsonProperty("labels")]
        public List<object> Labels { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("ratingCount")]
        public int RatingCount { get; set; }

        [JsonProperty("bestReview")]
        public object BestReview { get; set; }

        [JsonProperty("priceTag")]
        public PriceTag PriceTag { get; set; }

        [JsonProperty("balanceState")]
        public string BalanceState { get; set; }
    }
}
