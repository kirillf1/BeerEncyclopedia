using BeerShared.DTO;
using BeerShared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ShopBeerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopBeerBindController : ControllerBase
    {
        private readonly IShopBeerBinder shopBeerBinder;

        public ShopBeerBindController(IShopBeerBinder shopBeerBinder)
        {
            this.shopBeerBinder = shopBeerBinder;
        }
        [Route("Bind")]
        [HttpPost]
        public async Task<ActionResult> Bind(BindedSourceBeer bind)
        {
            await shopBeerBinder.BindBeers(bind, default);
            return Ok();
        }
        [Route("GetNotBindedShopBeers")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ShopBeerInfo>>> GetNotBindedShopBeers(SourceBeerDetailsToBind sourceBeerDetails)
        {
            var beers = await shopBeerBinder.GetNotBindedShopBeers(sourceBeerDetails, default);
            return Ok(beers);
        }
        [Route("TryBindShopBeers")]
        [HttpPost]
        public async Task<ActionResult<bool>> TryBindShopBeers(SourceBeerDetailsToBind sourceBeerDetails)
        {
            var result = await shopBeerBinder.TryBindShopBeers(sourceBeerDetails, default);
            return Ok(result);
        }
    }
}
