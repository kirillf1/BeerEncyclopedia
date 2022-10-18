using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BeerEncyclopedia.Application.BeersServices;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Application.Contracts.Manufacturers;
using BeerEncyclopedia.Application.ManufacturerServices;
using BeerShared.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeerEncyclopedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<ManufacturerLabel>>> GetManufacturers([FromQuery] ManufacturerQuery manufacturerQuery,
            [FromServices] IManufacturerSearchService manufacturerSearchService)
        {
            return this.ToActionResult(await manufacturerSearchService.SearchManufacturerLabel(manufacturerQuery, default));

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ManufacturerDetails>> GetDetails(Guid id,
          [FromServices] IManufacturerSearchService manufacturerSearchService)
        {
            return this.ToActionResult(await manufacturerSearchService.GetManufacturerDetails(id, default));
        }
    }
}
