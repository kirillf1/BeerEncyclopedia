using Ardalis.Result.AspNetCore;
using BeerEncyclopedia.Application.ColorServices;
using BeerEncyclopedia.Application.Contracts.Colors;
using Microsoft.AspNetCore.Mvc;

namespace BeerEncyclopedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDto>> GetColor(Guid id,
           [FromServices] IColorSearchService colorSearchService)
        {
            return this.ToActionResult(await colorSearchService.GetById(id));

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorDto>>> GetColors([FromQuery] string? name,
         [FromServices] IColorSearchService colorSearchService)
        {
            if (name is null)
                return this.ToActionResult(await colorSearchService.GetAllColors());
            return this.ToActionResult(await colorSearchService.GetByName(name));
        }
    }
}
