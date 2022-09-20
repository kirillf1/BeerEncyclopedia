using BeerShared.Data;
using BeerShared.DTO;
using BeerShared.Interfaces;
using BeerShared.Queries;
using Microsoft.AspNetCore.Mvc;
using ShopBeerService.Services;

namespace ShopBeerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopBeerController : ControllerBase
    {
        private readonly IBeerShopService beerService;
        public ShopBeerController(IBeerShopService beerService)
        {
            this.beerService = beerService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResult<ShopBeerInfo>>> GetBeers([FromQuery] ShopBeerQuery shopBeerQuery)
        {
            return await beerService.GetShopBeers(shopBeerQuery);
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ShopBeerInfo beerInfo)
        {
            await beerService.UpdateBeer(beerInfo);
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ShopBeerInfo beerInfo)
        {
            var isAdded = await beerService.AddBeer(beerInfo);
            if (!isAdded)
                return BadRequest("Beer invalid");
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            await beerService.DeleteBeer(id);
            return Ok();
        }
    }
}
