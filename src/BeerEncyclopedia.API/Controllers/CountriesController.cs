using Ardalis.Result.AspNetCore;
using BeerEncyclopedia.Application.Contracts.Countries;
using BeerEncyclopedia.Application.CountryServices;
using Microsoft.AspNetCore.Mvc;

namespace BeerEncyclopedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(Guid id,
           [FromServices] ICountrySearchService countrySearchService)
        {
            return this.ToActionResult(await countrySearchService.GetById(id));

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries([FromQuery] string? name,
         [FromServices] ICountrySearchService countrySearchService)
        {
            if (name is null)
                return this.ToActionResult(await countrySearchService.GetAllCountries());
            return this.ToActionResult(await countrySearchService.GetByName(name));
        }
    }
}
