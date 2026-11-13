using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.BusinessLogic.Handlers.Destinations;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for destinations.</summary>
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

        /// <summary>Retrieves a city.</summary>
        // GET api/destinations/cities/{cityId}
        [HttpGet("cities/{cityId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from GetCityHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCityResponse>> GetCity([FromRoute] int cityId, [FromServices] GetCityHandler getCityHandler)
        {
            GetCityResponse result = await getCityHandler.GetCityAsync(cityId);
            return Ok(result);
        }

        /// <summary>Lists activities belonging to a city.</summary>
        // GET api/destinations/cities/{cityId}/activities
        [HttpGet("cities/{cityId:int}/activities")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetCityActivitiesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetCityActivitiesResponse>>> GetActivities([FromRoute] int cityId, [FromServices] GetCityActivitiesHandler getCityActivitiesHandler)
        {
            List<GetCityActivitiesResponse> result = await getCityActivitiesHandler.GetCityActivitiesAsync(cityId);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        /// <summary>Retrieves one activity.</summary>
        // GET api/destinations/activities/{activityId}
        [HttpGet("activities/{activityId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetActivityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from GetActivityHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetActivityResponse>> GetActivity([FromRoute] int activityId, [FromServices] GetActivityHandler getActivityHandler)
        {
            GetActivityResponse result = await getActivityHandler.GetActivityAsync(activityId);
            return Ok(result);
        }

        /// <summary>Retrieves destination statistics.</summary>
        // GET api/destinations/cities/{cityId}/stats
        [HttpGet("cities/{cityId:int}/stats")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetDestinationStatsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from GetDestinationStatsHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetDestinationStatsResponse>> GetStats([FromRoute] int cityId, [FromServices] GetDestinationStatsHandler getDestinationStatsHandler)
        {
            GetDestinationStatsResponse result = await getDestinationStatsHandler.GetDestinationStatsAsync(cityId);
            return Ok(result);
        }

        /// <summary>Creates a city and returns its ID.</summary>
        // POST api/destinations/cities
        [HttpPost("cities")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // May be thrown from CreateCityHandler.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] // May be thrown from CreateCityHandler.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateCity([FromBody] CreateCityRequest request, [FromServices] CreateCityHandler createCityHandler)
        {
            int cityId = await createCityHandler.CreateCityAsync(request);
            return Created($"/api/destinations/cities/{cityId}", cityId);
        }

        /// <summary>Updates a city and returns its ID.</summary>
        // PUT api/destinations/cities/{cityId}
        [HttpPut("cities/{cityId:int}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // May be thrown from UpdateCityHandler.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from UpdateCityHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateCity([FromRoute] int cityId, [FromBody] UpdateCityRequest request, [FromServices] UpdateCityHandler updateCityHandler)
        {
            request.CityId = cityId;
            await updateCityHandler.UpdateCityAsync(request);
            return Ok(cityId);
        }

        /// <summary>Deletes a city and returns its ID.</summary>
        // DELETE api/destinations/cities/{cityId}
        [HttpDelete("cities/{cityId:int}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from DeleteCityHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status409Conflict)] // May be thrown from DeleteCityHandler.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeleteCity([FromRoute] int cityId, [FromServices] DeleteCityHandler deleteCityHandler)
        {
            await deleteCityHandler.DeleteCityAsync(cityId);
            return Ok(cityId);
        }

        /// <summary>Creates an activity and returns its ID.</summary>
        // POST api/destinations/activities
        [HttpPost("activities")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // May be thrown from CreateActivityHandler.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateActivity([FromBody] CreateActivityRequest request, [FromServices] CreateActivityHandler createActivityHandler)
        {
            int activityId = await createActivityHandler.CreateActivityAsync(request);
            return Created($"/api/destinations/activities/{activityId}", activityId);
        }

        /// <summary>Updates an activity and returns its ID.</summary>
        // PUT api/destinations/activities/{activityId}
        [HttpPut("activities/{activityId:int}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // May be thrown from UpdateActivityHandler.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from UpdateActivityHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateActivity([FromRoute] int activityId, [FromBody] UpdateActivityRequest request, [FromServices] UpdateActivityHandler updateActivityHandler)
        {
            request.ActivityId = activityId;
            await updateActivityHandler.UpdateActivityAsync(request);
            return Ok(activityId);
        }

        /// <summary>Deletes an activity and returns its ID.</summary>
        // DELETE api/destinations/activities/{activityId}
        [HttpDelete("activities/{activityId:int}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Thrown from DeleteActivityHandler for missing resources.
        [ProducesResponseType(StatusCodes.Status409Conflict)] // May be thrown from DeleteActivityHandler.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeleteActivity([FromRoute] int activityId, [FromServices] DeleteActivityHandler deleteActivityHandler)
        {
            await deleteActivityHandler.DeleteActivityAsync(activityId);
            return Ok(activityId);
        }
    }
}
