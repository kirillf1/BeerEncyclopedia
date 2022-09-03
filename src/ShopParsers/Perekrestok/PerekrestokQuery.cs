using Newtonsoft.Json;

namespace ShopParsers.Perekrestok
{
    public class PerekrestokQuery
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("perPage")]
        public int PerPage { get; set; }

        [JsonProperty("filter")]
        public Filter Filter { get; set; }
    }
    public class Filter
    {
        [JsonProperty("category")]
        public int Category { get; set; }
    }
}
