namespace ShopBeerService.Workers
{
    public record BeerShopServiceArgs(StartTimeServiceArgs StartTimeServiceArgs, int Threads, string ShopName);
}
