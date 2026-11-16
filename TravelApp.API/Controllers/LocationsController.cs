
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Locations.CreateCity;
using TravelApp.BusinessLogic.Handlers.Locations.DeleteCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetCities;
using TravelApp.BusinessLogic.Handlers.Locations.GetCity;
using TravelApp.BusinessLogic.Handlers.Locations.GetContinents;
using TravelApp.BusinessLogic.Handlers.Locations.GetCountries;
using TravelApp.BusinessLogic.Handlers.Locations.GetDestinationStats;
using TravelApp.BusinessLogic.Handlers.Locations.UpdateCity;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for geographic locations.</summary>
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        #region GET All - Cities, Countries, Continents

        /// <summary>Lists cities with optional filtering and sorting.</summary>
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

        #region GET By ID - Cities, Stats

        /// <summary>Retrieves a city.</summary>
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
        // GET api/locations/cities/{cityId}/stats
        [HttpGet("cities/{cityId:int}/stats")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCityStatsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCityStatsResponse>> GetStats([FromRoute] int cityId, [FromServices] GetCityStatsHandler getDestinationStatsHandler)
        {
            GetCityStatsResponse result = await getDestinationStatsHandler.GetCityStatsAsync(cityId);

            return Ok(result);
        }

        #endregion

        #region Create - Cities

        /// <summary>Creates a city and returns its ID.</summary>
        // POST api/locations/cities
        [HttpPost("cities")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateCity([FromBody] CreateCityRequest request, [FromServices] CreateCityHandler createCityHandler)
        {
            int cityId = await createCityHandler.CreateCityAsync(request);

            return Created($"/api/locations/cities/{cityId}", request);
        }

        #endregion

        #region Update - Cities

        /// <summary>Updates a city and returns its ID.</summary>
        // PUT api/locations/cities/{cityId}
        [HttpPut("cities/{cityId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateCity([FromRoute] int cityId, [FromBody] UpdateCityRequest request, [FromServices] UpdateCityHandler updateCityHandler)
        {
            await updateCityHandler.UpdateNameAndCountryAsync(cityId, request);

            return Ok(cityId);
        }

        #endregion

        #region Delete - Cities

        /// <summary>Deletes a city and returns its ID.</summary>
        // DELETE api/locations/cities/{cityId}
        [HttpDelete("cities/{cityId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeleteCity([FromRoute] int cityId, [FromServices] DeleteCityHandler deleteCityHandler)
        {
            await deleteCityHandler.DeleteCityAsync(cityId);

            return Ok(cityId);
        }

        #endregion
    }
}
