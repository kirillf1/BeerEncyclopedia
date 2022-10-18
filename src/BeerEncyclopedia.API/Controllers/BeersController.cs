using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BeerEncyclopedia.Application.BeersServices.EditorServices;
using BeerEncyclopedia.Application.BeersServices.SearchServices;
using BeerEncyclopedia.Application.Contracts.Beers;
using BeerEncyclopedia.Domain;
using BeerShared.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeerEncyclopedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDetails>> GetDetails(Guid id,
            [FromServices] IBeerSearchByIdService beerSearchService)
        {
            return this.ToActionResult(await beerSearchService.GetBeerDetailsAsync(id, default));
        }
        [Route("names")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerLabel>>> GetBeerLabelsByNames([FromQuery] IEnumerable<string> names, int? count,
            [FromServices] IBeerSearchByNamesService beerSearchByNamesService)
        {
            return this.ToActionResult(await beerSearchByNamesService.SearchBeerLabels(names, count ?? 10));
        }

        [Route("query")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<BeerLabel>>> GetBeerLabelsApiResult([FromQuery] BeersQuery beersQuery,
            [FromServices] IBeerSearchByQueryService beerSearchService)
        {
            return this.ToActionResult(await beerSearchService.SearchBeerLabels(beersQuery, default));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateBeer([FromBody] BeerDetails beer,
            [FromServices] IBeerEditorService beerEditor)
        {
            return this.ToActionResult(await beerEditor.UpdateBeer(beer));
        }
        [HttpPost]
        public async Task<ActionResult> AddBeer([FromBody] BeerDetails beer,
            [FromServices] IBeerEditorService beerEditor)
        {
            return this.ToActionResult(await beerEditor.AddBeer(beer));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveBeer(Guid id,
            [FromServices] IBeerEditorService beerEditor)
        {
            return this.ToActionResult(await beerEditor.RemoveBeer(id));
        }
    }
}
