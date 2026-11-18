using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Locations.CreateCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetCities;
using TravelApp.BusinessLogic.Handlers.Locations.GetCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetCityStats;
using TravelApp.BusinessLogic.Handlers.Locations.GetContinents;
using TravelApp.BusinessLogic.Handlers.Locations.GetCountries;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for geographic locations.</summary>
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {

        #region POST - Create City

        /// <summary>Creates a city and returns the created city.</summary>
        /// <param name="request">The city to create.</param>
        /// <param name="createCityHandler">The handler that creates the city.</param>
        /// <returns>A created response for the new city.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the country does not exist.</exception>
        // POST api/locations/cities
        [HttpPost("cities")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CreateCityResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateCityResponse>> CreateCity([FromBody] CreateCityRequest request, [FromServices] CreateCityHandler createCityHandler)
        {
            CreateCityResponse createdCity = await createCityHandler.CreateCityAsync(request);

            return Created($"/api/locations/cities/{createdCity.Id}", createdCity);
        }

        #endregion

        #region GET - All Cities, Countries & Continents

        /// <summary>Lists cities with optional filtering and sorting.</summary>
        /// <param name="request">The filter and sort options.</param>
        /// <param name="getCitiesHandler">The handler that loads the cities.</param>
        /// <returns>The matching cities.</returns>
        // GET api/locations/cities
        [HttpGet("cities")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetCitiesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetCitiesResponse>>> GetCities([FromQuery] GetCitiesRequest request, [FromServices] GetCitiesHandler getCitiesHandler)
        {
            List<GetCitiesResponse> result = await getCitiesHandler.GetCitiesAsync(request);

            return Ok(result);
        }

        /// <summary>Lists countries, optionally filtered by continent.</summary>
        /// <param name="continentId">The continent identifier to filter by, or <see langword="null"/> for all continents.</param>
        /// <param name="getCountriesHandler">The handler that loads the countries.</param>
        /// <returns>The matching countries.</returns>
        // GET api/locations/countries
        [HttpGet("countries")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetCountriesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetCountriesResponse>>> GetCountries([FromQuery] int? continentId, [FromServices] GetCountriesHandler getCountriesHandler)
        {
            List<GetCountriesResponse> result = await getCountriesHandler.GetCountriesAsync(continentId);

            return Ok(result);
        }

        /// <summary>Lists continents.</summary>
        /// <param name="getContinentsHandler">The handler that loads the continents.</param>
        /// <returns>The continents.</returns>
        // GET api/locations/continents
        [HttpGet("continents")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetContinentsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetContinentsResponse>>> GetContinents([FromServices] GetContinentsHandler getContinentsHandler)
        {
            List<GetContinentsResponse> result = await getContinentsHandler.GetContinentsAsync();

            return Ok(result);
        }

        #endregion

        #region GET - By ID Cities, Stats

        /// <summary>Retrieves a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="getCityHandler">The handler that loads the city.</param>
        /// <returns>The city.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city does not exist.</exception>
        // GET api/locations/cities/{cityId}
        [HttpGet("cities/{cityId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCityResponse>> GetCity([FromRoute] int cityId, [FromServices] GetCityHandler getCityHandler)
        {
            GetCityResponse result = await getCityHandler.GetCityAsync(cityId);

            return Ok(result);
        }

        /// <summary>Retrieves destination statistics for a city.</summary>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="getCityStatsHandler">The handler that loads the statistics.</param>
        /// <returns>The statistics for the city.</returns>
        /// <exception cref="TravelAppNotFoundException">Thrown when the city does not exist.</exception>
        // GET api/locations/cities/{cityId}/stats
        [HttpGet("cities/{cityId:int}/stats")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCityStatsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCityStatsResponse>> GetStats([FromRoute] int cityId, [FromServices] GetCityStatsHandler getCityStatsHandler)
        {
            GetCityStatsResponse result = await getCityStatsHandler.GetCityStatsAsync(cityId);

            return Ok(result);
        }

        #endregion
    }
}
