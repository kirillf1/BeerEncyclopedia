using BeerShared.DTO;
using ShopParsers;

namespace ShopBeerService.Converters
{
    public static class BeerInfoConverter
    {
        public static ShopBeer ConvertToShopBeer(ShopBeerInfo beer)
        {
            return new ShopBeer(beer.Name, beer.Price)
            {
                Id = beer.Id,
                Brand = beer.Brand,
                Description = beer.Description,
                ShopId = beer.ShopId.GetValueOrDefault(),
                SourceBeerId = beer.SourceBeerId,
                Strength = beer.Strength,
                Style = beer.Style,
                Color = beer.Color,
                Country = beer.Country,
                DiscountPrice = beer.DiscountPrice,
                Filtration = beer.Filtration,
                InitialWort = beer.InitialWort,
                Manufacturer = beer.Manufacturer,
                Name = beer.Name,
                Pasteurization = beer.Pasteurization,
                Price = beer.Price,
                Rating = beer.Rating,
                Volume = beer.Volume
            };
        }
        public static ShopBeerInfo ConvertToShopBeerInfo(ShopBeer beer)
        {
            return new ShopBeerInfo
            {
                Id = beer.Id,
                Brand = beer.Brand,
                Description = beer.Description,
                ShopId = beer.ShopId,
                SourceBeerId = beer.SourceBeerId,
                Strength = beer.Strength,
                Style = beer.Style,
                Color = beer.Color,
                Country = beer.Country,
                DiscountPrice = beer.DiscountPrice,
                Filtration = beer.Filtration,
                InitialWort = beer.InitialWort,
                Manufacturer = beer.Manufacturer,
                Name = beer.Name,
                Pasteurization = beer.Pasteurization,
                Price = beer.Price,
                Rating = beer.Rating,
                Volume = beer.Volume
            };
        }
    }
}

