﻿using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Domain;

namespace BeerEncyclopedia.Application.Helpers
{
    public static class ManufactureDtoConverter
    {
        public static ManufacturerLabel ConvertManufacturerToLabel(Manufacturer manufacturer)
        {
            return new ManufacturerLabel
            {   
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                PictureUrl = manufacturer.PictureUrl,
                Country = CountryDtoConverter.ConvertCountryToDto(manufacturer.Country)
            };
        }
        public static ManufacturerDetails ConvertManufacturerToDetails(Manufacturer manufacturer,int beerCount = 10)
        {
            return new ManufacturerDetails
            {
                Country = CountryDtoConverter.ConvertCountryToDto(manufacturer.Country),
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                PictureUrl = manufacturer.PictureUrl,
                Description = manufacturer.Description,
                Beers = manufacturer.Beers.Select(b=> BeerDtoConventer.ConvertBeerToLabel(b)).Take(beerCount)
                
            };
        }
    }
}
