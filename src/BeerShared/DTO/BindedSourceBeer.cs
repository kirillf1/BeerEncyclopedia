namespace BeerShared.DTO
{
    public class BindedSourceBeer
    {
        public BindedSourceBeer(Guid sourceBeerId, List<int> shopBeerIds)
        {
            SourceBeerId = sourceBeerId;
            ShopBeerIds = shopBeerIds;
        }
        public Guid SourceBeerId { get; set; }
        public List<int> ShopBeerIds { get; set; }
    }
}
