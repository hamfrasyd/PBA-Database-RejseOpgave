using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.ExceptionFilters;
using TravelApp.BusinessLogic.Handlers.Activities.GetActivity;
using TravelApp.BusinessLogic.Handlers.Trips;
using TravelApp.BusinessLogic.Handlers.Trips.CreateTrip;
using TravelApp.BusinessLogic.Handlers.Trips.GetTrip;
using TravelApp.BusinessLogic.Handlers.Trips.TripParticipants;

namespace TravelApp.API.Controllers
{
    /// <summary>Exposes HTTP endpoints for trips and their related resources.</summary>
    [ApiController]
    [Authorize]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        /// <summary>Lists trips belonging to the current user.</summary>
        // GET api/trips
        [HttpGet]
        [ProducesResponseType(typeof(List<GetTripsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetTripsResponse>>> GetTrips([FromQuery] int? countryId, [FromQuery] int? continentId, [FromServices] GetTripsHandler handler)
        {
            List<GetTripsResponse> result = await handler.GetTripsAsync(countryId, continentId);
            return Ok(result);
        }

        /// <summary>Retrieves a trip with its cities, countries and continents.</summary>
        // GET api/trips/{tripId}
        [HttpGet("{tripId:int}")]
        [ProducesResponseType(typeof(GetTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTripResponse>> GetTrip([FromRoute] int tripId, [FromServices] GetTripHandler getTripHandler)
        {
            GetTripResponse result = await getTripHandler.GetTripAsync(tripId);
            return Ok(result);
        }

        /// <summary>Lists activities associated with a trip.</summary>
        // GET api/trips/{tripId}/activities
        [HttpGet("{tripId:int}/activities")]
        [ProducesResponseType(typeof(List<GetActivityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetActivityResponse>>> GetTripActivities([FromRoute] int tripId, [FromServices] GetTripActivitiesHandler getTripActivitiesHandler)
        {
            List<GetActivityResponse> result = await getTripActivitiesHandler.GetTripActivitiesAsync(tripId);

            return Ok(result);
        }

        /// <summary>Lists users participating in a trip.</summary>
        // GET api/trips/{tripId}/participants
        [HttpGet("{tripId:int}/participants")]
        [ProducesResponseType(typeof(List<GetTripParticipantResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetTripParticipantResponse>>> GetTripParticipants([FromRoute] int tripId, [FromServices] GetTripParticipantsHandler getTripParticipantsHandler)
        {
            List<GetTripParticipantResponse> result = await getTripParticipantsHandler.GetTripParticipantsAsync(tripId);

            return Ok(result);
        }

        /// <summary>Creates a trip for the current user.</summary>
        // POST api/trips
        [HttpPost]
        [ProducesResponseType(typeof(GetTripResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateTripResponse>> CreateTrip([FromBody] CreateTripRequest request, [FromServices] CreateTripHandler createTripHandler)
        {
            CreateTripResponse result = await createTripHandler.CreateTripAsync(request);
            return Created($"/api/trips/{result.Id}", result);
        }

        /// <summary>Updates a trip's dates, status and notes.</summary>
        // PUT api/trips/{tripId}
        [HttpPut("{tripId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrip([FromRoute] int tripId, [FromBody] UpdateTripRequest request, [FromServices] UpdateTripHandler updateTripHandler)
        {
            request.TripId = tripId;
            await updateTripHandler.UpdateTripAsync(request);
            return NoContent();
        }

        /// <summary>Deletes a trip and its related records.</summary>
        // DELETE api/trips/{tripId}
        [HttpDelete("{tripId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTrip([FromRoute] int tripId, [FromServices] DeleteTripHandler deleteTripHandler)
        {
            await deleteTripHandler.DeleteTripAsync(tripId);
            return NoContent();
        }

        /// <summary>Adds an existing city to a trip.</summary>
        // POST api/trips/{tripId}/cities/{cityId}
        [HttpPost("{tripId:int}/cities/{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCity([FromRoute] int tripId, [FromRoute] int cityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.AddCityAsync(tripId, cityId);
            return NoContent();
        }

        /// <summary>Removes a city from a trip.</summary>
        // DELETE api/trips/{tripId}/cities/{cityId}
        [HttpDelete("{tripId:int}/cities/{cityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCity([FromRoute] int tripId, [FromRoute] int cityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.RemoveCityAsync(tripId, cityId);
            return NoContent();
        }

        /// <summary>Adds an existing activity to a trip.</summary>
        // POST api/trips/{tripId}/activities/{activityId}
        [HttpPost("{tripId:int}/activities/{activityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddActivity([FromRoute] int tripId, [FromRoute] int activityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.AddActivityAsync(tripId, activityId);
            return NoContent();
        }

        /// <summary>Removes an activity from a trip.</summary>
        // DELETE api/trips/{tripId}/activities/{activityId}
        [HttpDelete("{tripId:int}/activities/{activityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveActivity([FromRoute] int tripId, [FromRoute] int activityId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.RemoveActivityAsync(tripId, activityId);
            return NoContent();
        }

        /// <summary>Adds an existing user as a trip participant.</summary>
        // POST api/trips/{tripId}/participants/{userId}
        [HttpPost("{tripId:int}/participants/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddParticipant([FromRoute] int tripId, [FromRoute] int userId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.AddParticipantAsync(tripId, userId);
            return NoContent();
        }

        /// <summary>Removes a participant from a trip.</summary>
        // DELETE api/trips/{tripId}/participants/{userId}
        [HttpDelete("{tripId:int}/participants/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveParticipant([FromRoute] int tripId, [FromRoute] int userId, [FromServices] ManageTripRelationsHandler manageTripRelationsHandler)
        {
            await manageTripRelationsHandler.RemoveParticipantAsync(tripId, userId);
            return NoContent();
        }
    }
}
