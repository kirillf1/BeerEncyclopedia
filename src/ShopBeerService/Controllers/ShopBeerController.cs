using BeerShared.Data;
using BeerShared.DTO;
using Microsoft.AspNetCore.Mvc;
using ShopBeerService.Queries;
using ShopBeerService.Services;

namespace ShopBeerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopBeerController : ControllerBase
    {
        private readonly BeerService beerService;
        public ShopBeerController(BeerService beerService)
        {
            this.beerService = beerService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResult<ShopBeerInfo>>> GetBeers([FromQuery]ShopBeerQuery shopBeerQuery)
        {
            return await beerService.GetShopBeers(shopBeerQuery);
        }
        [HttpDelete]
        public async Task<ActionResult> Remove([FromQuery] Guid? shopId, [FromQuery] string beerName)
        {
            await beerService.DeleteBeer(shopId, beerName);
            return Ok();
        }
    }
}
