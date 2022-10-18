using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BeerEncyclopedia.Application.Contracts.Styles;
using BeerEncyclopedia.Application.StyleServices;
using BeerShared.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeerEncyclopedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StylesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<StyleDetails>> GetDetails(Guid id,
            [FromServices] IStyleSearchService styleSearchService)
        {
            return this.ToActionResult(await styleSearchService.GetStyleDetails(id, default));

        }
        [HttpGet]
        public async Task<ActionResult<ApiResult<StyleLabel>>> GetStyleLabels([FromQuery] StyleQuery styleQuery,
            [FromServices] IStyleSearchService styleSearchService)
        {
            return this.ToActionResult(await styleSearchService.SearchStyleLabel(styleQuery, default));
        }
    }
}
