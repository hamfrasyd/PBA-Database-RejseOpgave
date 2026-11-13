using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.Destinations;

namespace TravelApp.API.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public class DestinationsController : ControllerBase
    {
        /// <summary>Lists continents.</summary>
        // GET api/destinations/continents
        [HttpGet("continents")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetContinentsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetContinentsResponse>>> GetContinents([FromServices] GetContinentsHandler getContinentsHandler)
        {
            List<GetContinentsResponse> result = await getContinentsHandler.GetContinentsAsync();
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>Lists countries, optionally filtered by continent.</summary>
        // GET api/destinations/countries
        [HttpGet("countries")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetCountriesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetCountriesResponse>>> GetCountries([FromQuery] int? continentId, [FromServices] GetCountriesHandler getCountriesHandler)
        {
            List<GetCountriesResponse> result = await getCountriesHandler.GetCountriesAsync(continentId);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>Lists cities with optional filtering and sorting.</summary>
        // GET api/destinations/cities
        [HttpGet("cities")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetCitiesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetCitiesResponse>>> GetCities([FromQuery] GetCitiesRequest request, [FromServices] GetCitiesHandler getCitiesHandler)
        {
            List<GetCitiesResponse> result = await getCitiesHandler.GetCitiesAsync(request);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
